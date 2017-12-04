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
    Constantine = "constantine"
    Derwent = "derwent"
    Goodricke = "goodricke"
    Halifax = "halifax"
    James = "james"
    Langwith = "langwith"
    Vanbrugh = "vanbrugh"
    Wentworth = "wentworth"
    Concrete = "concrete"
    Grass = "grass"
    Stones = "stones"
    Water = "water"


IMAGE_KEY_CONV = {
    pygame.K_0: None,
    pygame.K_1: Images.Alcuin,
    pygame.K_2: Images.Constantine,
    pygame.K_3: Images.Derwent,
    pygame.K_4: Images.Goodricke,
    pygame.K_5: Images.Halifax,
    pygame.K_6: Images.James,
    pygame.K_7: Images.Langwith,
    pygame.K_8: Images.Vanbrugh,
    pygame.K_9: Images.Wentworth,
    pygame.K_c: Images.Concrete,
    pygame.K_g: Images.Grass,
    pygame.K_s: Images.Stones,
    pygame.K_w: Images.Water,
}

DATA_KEY_CONV = {
    pygame.K_EQUALS: True,
    pygame.K_MINUS: False
}


hex_images_paths = {}
for img in Images:
    hex_images_paths[img] = os.path.join(IMAGE_DIR, img.value + ".png")

hex_images = {}

DEF_GRID = {
    hex.Coord(0, 0): (Images.Grass, True),
    hex.Coord(1, 0): (Images.Water, True),
    hex.Coord(0, 1): (Images.Concrete, True),
    hex.Coord(-1, 0): (Images.Alcuin, True),
    hex.Coord(0, -1): (Images.Constantine, True),
    hex.Coord(1, -1): (Images.Derwent, True),
    hex.Coord(-1, 1): (Images.Langwith, True)
}

TRAVERSABLE_IGNORE_IMAGES = [
    Images.Water
]


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

class Cross(game_lib.RenderableObject, game_lib.RectObject):
    def __init__(self, w, h, width=1):
        self._rect = pygame.Rect(0, 0, w, h)
        self.width = width
    
    def render(self, screen, camera):
        rect = self._get_draw_rect(self._rect, camera)
        pygame.draw.line(screen, (200, 0, 0), rect.topleft, rect.bottomright, self.width)
        pygame.draw.line(screen, (200, 0, 0), rect.topright, rect.bottomleft, self.width)


class Sector(game_lib.RenderableSurface):
    def __init__(self, img, layout, pos=None, traversable=True):
        super().__init__(hex_images[img])
        self._change_size.pop()
        self.__img = img
        self.__layout = layout
        self.__offsetx = layout.origin.x / 2
        self.__offsety = layout.origin.y
        self.size = (layout.size.x, layout.size.y)
        self.pos = pos if pos is not None else hex.Coord(0, 0)
        self.traversable = traversable
        self.__cross = Cross(self.w * 0.6, self.h * 0.6, 2)
        self.__cross._mode = game_lib.RenderMode.World
        self.__cross.c = self.c
    
    def render(self, screen, camera):
        super().render(screen, camera)
        if not self.traversable:
            self.__cross.render(screen, camera)

    def set_image(self, img):
        self.img = img

    @property
    def img(self):
        return self.__img

    @img.setter
    def img(self, value):
        self.__img = value
        self._surface = hex_images[value]

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
        
        # init hex surfaces
        tmpSurface = None
        for k, v in hex_images_paths.items():
            if tmpSurface is None:
                tmpSurface = game_lib.RenderableImage(v)
                tmpSurface.s = (self.layout.size.x, self.layout.size.y)
            else:
                tmpSurface.set_image(v)
            hex_images[k] = tmpSurface._surface

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
                Sector, v[0], self.layout, k, v[1])
        return parsed

    def init_gui(self):
        self.buttons = {}
        lby = 20
        lbspace = 10
        btn = self.renderer.init_screen_obj(
            Button, "Save", (255, 255, 255), (150, 150, 150), border=(100, 100, 100), border_width=3)
        btn.w = 60
        btn.h = 30
        btn.x = WIDTH - btn.w - 20
        btn.y = lby
        lby += btn.h
        self.buttons["save"] = btn

        # add hexagon management buttons
        hex_img_attr = "img_to_set"
        hex_button_none = self.get_hex_image_button_name("None")
        btn = self.renderer.init_screen_obj(
            Button, "None (0)", (255, 255, 255), (50, 50, 50), border_width=0)
        btn.x = 10
        btn.y = 10
        btn.w = 110
        btn.h = 40
        btn.active = False
        setattr(btn, hex_img_attr, None)
        self.buttons[hex_button_none] = btn
        self.hex_image_buttons = {}
        self.hex_image_buttons_lst = []
        self.add_hex_image_button(Images.Alcuin, (214, 52, 79), False, "1")
        self.add_hex_image_button(Images.Constantine, (237, 105, 222), False, "2")
        self.add_hex_image_button(Images.Derwent, (104, 144, 238), False, "3")
        self.add_hex_image_button(Images.Goodricke, (64, 238, 125), False, "4")
        self.add_hex_image_button(Images.Halifax, (24, 222, 173), False, "5")
        self.add_hex_image_button(Images.James, (39, 33, 92), True, "6")
        self.add_hex_image_button(Images.Langwith, (216, 236, 65), False, "7")
        self.add_hex_image_button(Images.Vanbrugh, (120, 51, 128), False, "8")
        self.add_hex_image_button(Images.Wentworth, (77, 67, 39), True, "9")
        self.add_hex_image_button(Images.Concrete, (131, 129, 129), False, "c")
        self.add_hex_image_button(Images.Grass, (48, 86, 27), False, "g")
        self.add_hex_image_button(Images.Stones, (118, 114, 104), False, "s")
        self.add_hex_image_button(Images.Water, (64, 104, 124), False, "w")
        hbspace = 5
        hbw = btn.w
        hbh = btn.h
        hbx = btn.x
        hby = btn.y + hbh + hbspace
        for k in self.hex_image_buttons_lst:
            v = self.hex_image_buttons[k]
            # init button
            btn = self.renderer.init_screen_obj(
                Button, "{} ({})".format(v[0].name, v[3]), v[1], v[2], border_width=0)
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
        self.hex_image_buttons[hex_button_none] = (None, btn.fg, btn.bg, "0")

        # hex data buttons
        hex_data_attr = "traversal_value"
        self.hex_data_buttons = []
        lby += lbspace
        name = self.get_hex_image_button_name("TraversableTrue")
        btn = self.renderer.init_screen_obj(
            Button, "Traversable True (=)", (255, 255, 255), (50, 50, 50), border_width=0)
        btn.w = 150
        btn.h = 40
        btn.x = WIDTH - btn.w - 20
        btn.y = lby
        lby += btn.h
        btn.active = False
        setattr(btn, hex_data_attr, True)
        self.buttons[name] = btn
        self.hex_data_buttons.append(name)
        lby += lbspace
        name = self.get_hex_image_button_name("TraversableFalse")
        btn = self.renderer.init_screen_obj(
            Button, "Traversable False (-)", (255, 255, 255), (50, 50, 50), border_width=0)
        btn.w = 150
        btn.h = 40
        btn.x = WIDTH - btn.w - 20
        btn.y = lby
        btn.active = False
        setattr(btn, hex_data_attr, False)
        self.buttons[name] = btn
        self.hex_data_buttons.append(name)

    def get_hex_image_button_name(self, img):
        return "sethex-{}".format(img)

    def add_hex_image_button(self, img, color, white, shortcut):
        name = self.get_hex_image_button_name(img)
        self.hex_image_buttons[name] = (
            img, (255, 255, 255) if white else (0, 0, 0), color, shortcut)
        self.hex_image_buttons_lst.append(name)

    def set_hex_button_active(self, active):
        for k in self.hex_image_buttons.keys():
            self.buttons[k].active = active
        for i in self.hex_data_buttons:
            self.buttons[i].active = active

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

    def set_current_hex_img(self, img):
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
    
    def set_current_hex_data(self, data):
        print("setting hex", self.current_hex, "traversable to", data)
        if self.current_hex in self.grid:
            self.grid[self.current_hex].traversable = data

    def do_events(self):
        for event in pygame.event.get():
            if event.type == pygame.QUIT:
                self.save_grid(self.args.output)
                sys.exit()
            elif event.type == pygame.KEYDOWN:
                if self.current_hex is not None:
                    if event.key in IMAGE_KEY_CONV:
                        self.set_current_hex_img(IMAGE_KEY_CONV[event.key])
                    elif event.key in DATA_KEY_CONV:
                        self.set_current_hex_data(DATA_KEY_CONV[event.key])
            elif event.type == pygame.MOUSEBUTTONDOWN:
                if event.button == 1:
                    pressed = self.get_button_pressed(event)
                    if pressed is not None:
                        if pressed == "save":
                            self.save_grid(self.args.output)
                        elif self.current_hex is not None:
                            if pressed in self.hex_image_buttons:
                                self.set_current_hex_img(
                                    self.buttons[pressed].img_to_set)
                            elif pressed in self.hex_data_buttons:
                                self.set_current_hex_data(
                                    self.buttons[pressed].traversal_value)
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
            traversable = True
            if "traversable" in item:
                traversable = item["traversable"]
            grid[hex.Coord.from_serializable(
                item["coordinate"])] = (Images[item["texture"]], traversable)
        return grid

    def save_grid(self, file):
        print("saving map to", file)
        map = {}
        gridlst = []
        for k, v in self.grid.items():
            sector = {
                "coordinate": k.to_serializable(),
                "texture": v.img.name
            }
            if not v.img in TRAVERSABLE_IGNORE_IMAGES:
                sector["traversable"] = v.traversable
            gridlst.append(sector)
        print("\t", len(gridlst), "sectors set")
        map["sectors"] = gridlst
        with open(file, "w") as fp:
            json.dump(map, fp)
        print("saved")


if __name__ == "__main__":
    App()
