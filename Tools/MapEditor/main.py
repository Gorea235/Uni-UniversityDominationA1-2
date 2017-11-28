#! /usr/bin/env python3
import argparse
import json
import math
import os
import sys
from enum import Enum
import pygame

import game_lib
import hex

pygame.init()

DEF_OUT = "output.json"

IMAGE_DIR = "images"
WINDOW_TITLE = "Map Editor"

SIZE = WIDTH, HEIGHT = 1000, 800
HEX_SIZE = hex.Point(70, 70)
# HEX_ORIGIN = hex.Point(int((WIDTH / 2) - (HEX_SIZE.x / 2)),
#                        int((HEIGHT / 2) - (HEX_SIZE.y / 2)))
HEX_ORIGIN = hex.Point(WIDTH // 2, HEIGHT // 2)


class Images(Enum):
    Alcuin = "alcuin"
    Concrete = "concrete"
    Constantine = "constantine"
    Derwent = "derwent"
    Goodricke = "goodricke"
    Grass = "grass"
    Halifax = "halifax"
    James = "james"
    Langwith = "langwith"
    Stones = "stones"
    Vanbrugh = "vanbrugh"
    Water = "water"
    Wentworth = "wentworth"


hex_images = {}
for img in Images:
    hex_images[img] = os.path.join(IMAGE_DIR, img.value + ".png")

DEF_GRID = {
    hex.Coord(0, 0): Images.Grass,
    hex.Coord(1, 0): Images.Water,
    hex.Coord(0, 1): Images.Concrete,
    hex.Coord(-1, 0): Images.Alcuin,
    hex.Coord(0, -1): Images.Constantine,
    hex.Coord(1, -1): Images.Derwent,
    hex.Coord(-1, 1): Images.Langwith
}


class Button(game_lib.RenderableObject, game_lib.RectObject):
    def __init__(self, text, fg, bg, border=(0, 0, 0), border_width=5):
        self.__mode = None
        self.__text = game_lib.RenderableText(text, fg, bg)
        self.__brect = game_lib.RenderableRect(bg, border, border_width)
        self._rect = self.__brect._rect
        self._change_pos.append(self.__reset_text_pos__)
        self._change_size.append(self.__reset_text_pos__)

    def hit(self, pos):
        return self.active and self._rect.collidepoint(pos)

    @property
    def _mode(self):
        return self.__mode

    @_mode.setter
    def _mode(self, value):
        self.__mode = value
        self.__text._mode = value
        self.__brect._mode = value

    def render(self, screen, camera):
        self.__brect.render(screen, camera)
        self.__text.render(screen, camera)

    def __reset_text_pos__(self):
        self.__text.cx = self.cx
        self.__text.cy = self.cy

    @property
    def text(self):
        return self.__text.text

    @text.setter
    def text(self, value):
        self.__text.text = value
        self.__reset_text_pos__()

    @property
    def fg(self):
        return self.__text.fg

    @fg.setter
    def fg(self, value):
        self.__text.fg = value

    @property
    def bg(self):
        return self.__brect.color

    @bg.setter
    def bg(self, value):
        self.__brect.color = value
        self.__text.bg = value

    @property
    def border(self):
        return self.__brect.border_color

    @border.setter
    def border(self, value):
        self.__brect.border_color = value

    @property
    def border_width(self):
        return self.__brect.border_width

    @border_width.setter
    def border_width(self, value):
        self.__brect.border_width = value


class Sector(game_lib.RenderableImage):
    def __init__(self, img, layout, pos=None):
        super().__init__(hex_images[img])
        self.__img = img
        self.__layout = layout
        self.__offsetx = layout.origin.x / 2
        self.__offsety = layout.origin.y
        self.size = (layout.size.x, layout.size.y)
        self.pos = pos if pos is not None else hex.Coord(0, 0)

    def set_image(self, img):
        self.img = img

    @property
    def img(self):
        return self.__img

    @img.setter
    def img(self, value):
        self.__img = value
        super().set_image(hex_images[value])

    @property
    def pos(self):
        return self.__pos

    @pos.setter
    def pos(self, value):
        self.__pos = value
        p = self.__layout.hex_to_pixel(value)
        self.cx = (p.x / 2) + self.__offsetx
        self.cy = (-p.y / 2) + self.__offsety


class App():
    def __init__(self):
        # init base vars
        self.screen = pygame.display.set_mode(SIZE)
        pygame.display.set_caption(WINDOW_TITLE)
        self.renderer = game_lib.Renderer(self.screen)
        self.layout = hex.Layout(
            hex.orientation_pointy, HEX_SIZE, HEX_ORIGIN)

        # setup cmd parser for input and output files
        self.parser = argparse.ArgumentParser(
            prog="UniDom Map Editor",
            description="A program that allows easy editing of the map that is loaded into the Unity UniDom game.")
        self.parser.add_argument("--input", "-i")
        self.parser.add_argument("--output", "-o", default=DEF_OUT)
        self.args = self.parser.parse_args()

        # setup grid using default or load from input file
        toparse = None
        if self.args.input is None:
            toparse = DEF_GRID
        else:
            toparse = self.load_grid(self.args.input)
        self.grid = self.parse_grid(toparse)

        # setup GUI
        self.init_gui()

        # init state tracking vars
        self.cam_moving = False
        self.current_hex = None

        print("init complete")
        # do main loop
        self.loop()

    def parse_grid(self, grid):
        parsed = {}
        for k, v in grid.items():
            parsed[k] = self.renderer.init_world_obj(
                Sector, v, self.layout, k)
        return parsed

    def init_gui(self):
        self.buttons = {}
        btn = self.renderer.init_screen_obj(
            Button, "Save", (255, 255, 255), (150, 150, 150), border=(100, 100, 100), border_width=3)
        btn.w = 60
        btn.h = 30
        btn.x = WIDTH - btn.w - 20
        btn.y = 20
        self.buttons["save"] = btn

        # add hexagon management buttons
        hex_img_attr = "img_to_set"
        hex_button_none = self.get_hex_image_button_name("None")
        btn = self.renderer.init_screen_obj(
            Button, "None", (255, 255, 255), (50, 50, 50), border_width=0)
        btn.x = 10
        btn.y = 10
        btn.w = 80
        btn.h = 40
        btn.active = False
        setattr(btn, hex_img_attr, None)
        self.buttons[hex_button_none] = btn
        self.hex_image_buttons = {}
        self.add_hex_image_button(Images.Alcuin, (214, 52, 79), False)
        self.add_hex_image_button(Images.Concrete, (131, 129, 129), False)
        self.add_hex_image_button(Images.Constantine, (237, 105, 222), False)
        self.add_hex_image_button(Images.Derwent, (104, 144, 238), False)
        self.add_hex_image_button(Images.Goodricke, (64, 238, 125), False)
        self.add_hex_image_button(Images.Grass, (48, 86, 27), False)
        self.add_hex_image_button(Images.Halifax, (24, 222, 173), False)
        self.add_hex_image_button(Images.James, (39, 33, 92), True)
        self.add_hex_image_button(Images.Langwith, (216, 236, 65), False)
        self.add_hex_image_button(Images.Stones, (118, 114, 104), False)
        self.add_hex_image_button(Images.Vanbrugh, (120, 51, 128), False)
        self.add_hex_image_button(Images.Water, (64, 104, 124), False)
        self.add_hex_image_button(Images.Wentworth, (77, 67, 39), True)
        hbspace = 5
        hbw = btn.w
        hbh = btn.h
        hbx = btn.x
        hby = btn.y + hbh + hbspace
        for k, v in self.hex_image_buttons.items():
            # init button
            btn = self.renderer.init_screen_obj(
                Button, v[0].name, v[1], v[2], border_width=0)
            # position button
            btn.x = hbx
            btn.y = hby
            btn.w = hbw
            btn.h = hbh
            hby += hbh + hbspace
            # config button
            btn.active = False
            setattr(btn, hex_img_attr, v[0])
            # save button
            self.buttons[k] = btn
        self.hex_image_buttons[hex_button_none] = (None, btn.fg, btn.bg)

    def get_hex_image_button_name(self, img):
        return "sethex-{}".format(img)

    def add_hex_image_button(self, img, color, white):
        self.hex_image_buttons[self.get_hex_image_button_name(img)] = (
            img, (255, 255, 255) if white else (0, 0, 0), color)

    def set_hex_button_active(self, active):
        for k in self.hex_image_buttons.keys():
            self.buttons[k].active = active

    def get_hex_at(self, pos):
        point = hex.Point((pos[0] - (self.layout.origin.x / 2)) * 2,
                          -(pos[1] - (self.layout.origin.y)) * 2)
        pos = self.layout.pixel_to_hex(point)
        return round(pos)

    def get_button_pressed(self, event):
        for k, v in self.buttons.items():
            if v.hit(event.pos):
                return k
        return None

    def do_events(self):
        for event in pygame.event.get():
            if event.type == pygame.QUIT:
                self.save_grid(self.args.output)
                sys.exit()
            elif event.type == pygame.KEYDOWN:
                pass
            elif event.type == pygame.MOUSEBUTTONDOWN:
                if event.button == 1:
                    pressed = self.get_button_pressed(event)
                    if pressed is not None:
                        if pressed == "save":
                            self.save_grid(self.args.output)
                        elif self.current_hex is not None and pressed in self.hex_image_buttons:
                            img = self.buttons[pressed].img_to_set
                            print("setting hex", self.current_hex, "to", img)
                            if img is None:
                                if self.current_hex in self.grid:
                                    self.renderer.remove_obj(
                                        self.grid[self.current_hex])
                                    del self.grid[self.current_hex]
                            elif self.current_hex in self.grid:
                                self.grid[self.current_hex].img = img
                            else:
                                self.grid[self.current_hex] = self.renderer.init_world_obj(
                                    Sector, img, self.layout, self.current_hex)
                            self.current_hex = None
                            self.set_hex_button_active(False)
                    else:
                        self.current_hex = self.get_hex_at(
                            self.renderer.camera.screen_to_world(*event.pos))
                        self.set_hex_button_active(True)
                        print("current hex is", self.current_hex)
                elif event.button == 3:
                    self.cam_moving = True
                elif event.button == 4:
                    pass  # zoom in
                elif event.button == 5:
                    pass  # zoom out
            elif event.type == pygame.MOUSEBUTTONUP:
                if event.button == 3:
                    self.cam_moving = False

    def do_mouse_processing(self):
        rx, ry = pygame.mouse.get_rel()
        if self.cam_moving:
            self.renderer.camera.x -= rx
            self.renderer.camera.y -= ry

    def loop(self):
        print("game loop started")
        while True:
            self.do_events()
            self.do_mouse_processing()
            self.renderer.perform_render()

    def load_grid(self, file):
        map = None
        with open(file) as fp:
            map = json.load(fp)
        grid = {}
        for item in map["sectors"]:
            grid[hex.Coord.from_serializable(
                item["coordinate"])] = Images[item["texture"]]
        return grid

    def save_grid(self, file):
        print("saving map to", file)
        map = {}
        gridlst = []
        for k, v in self.grid.items():
            gridlst.append({
                "coordinate": k.to_serializable(),
                "texture": v.img.name
            })
        print("\t", len(gridlst), "sectors set")
        map["sectors"] = gridlst
        with open(file, "w") as fp:
            json.dump(map, fp)
        print("saved")


if __name__ == "__main__":
    App()
