#! /usr/bin/env python3
import math
import os
import sys
from enum import Enum
import pygame

import game_lib
import hex

pygame.init()

IMAGE_DIR = "images"
WINDOW_TITLE = "Map Editor"
SIZE = WIDTH, HEIGHT = 600, 600
HEX_SIZE = hex.Point(100, 100)
# HEX_ORIGIN = hex.Point(int((WIDTH / 2) - (HEX_SIZE.x / 2)),
#                        int((HEIGHT / 2) - (HEX_SIZE.y / 2)))
HEX_ORIGIN = hex.Point(WIDTH // 2, HEIGHT // 2)


class Images(Enum):
    Alcuin = "alcuin.png"
    Concrete = "concrete.png"
    Constantine = "constantine.png"
    Derwent = "derwent.png"
    Goodricke = "goodricke.png"
    Grass = "grass.png"
    Halifax = "halifax.png"
    James = "james.png"
    Langwith = "langwith.png"
    Stones = "stones.png"
    Vanbrugh = "vanbrugh.png"
    Water = "water.png"
    Wentworth = "wentworth.png"


hex_images = {}
for img in Images:
    hex_images[img] = os.path.join(IMAGE_DIR, img.value)

DEF_GRID = {
    hex.Coord(0, 0): Images.Grass,
    hex.Coord(1, 0): Images.Water,
    hex.Coord(0, 1): Images.Concrete,
    hex.Coord(-1, 0): Images.Alcuin,
    hex.Coord(0, -1): Images.Constantine,
    hex.Coord(1, -1): Images.Derwent,
    hex.Coord(-1, 1): Images.Langwith
}


class Sector(game_lib.RenderableImage):
    def __init__(self, imgpath, layout, pos=None):
        super().__init__(imgpath)
        self.__layout = layout
        self.size = (layout.size.x, layout.size.y)
        self.pos = pos if pos is not None else hex.Coord(0, 0)

    @property
    def pos(self):
        return self.__pos

    @pos.setter
    def pos(self, value):
        self.__pos = value
        p = self.__layout.hex_to_pixel(value)
        self.cx = (p.x / 2) + (self.__layout.origin.x / 2)
        self.cy = (p.y / 2) + (self.__layout.origin.y / 2)


class App():
    def __init__(self):
        self.screen = pygame.display.set_mode(SIZE)
        pygame.display.set_caption(WINDOW_TITLE)
        self.renderer = game_lib.Renderer(self.screen)
        self.layout = hex.Layout(
            hex.orientation_pointy, HEX_SIZE, HEX_ORIGIN)
        self.grid = {}
        if True:  # todo: add sys.argv checking for loading json
            for k, v in DEF_GRID.items():
                self.grid[k] = self.renderer.instantiate(
                    Sector, hex_images[v], self.layout, k)
            # origin = hex.Coord(0, 0)
            # self.grid[origin] = self.renderer.instantiate(
            # Sector, hex_images[Images.Grass], self.layout, origin)  #  todo:
            # setup hex images
        self.loop()

    def get_hex_at(self, pos):
        point = hex.Point((pos[0] - (self.layout.origin.x / 2)) * 2,
                          (pos[1] - (self.layout.origin.y / 2)) * 2)
        pos = self.layout.pixel_to_hex(point)
        return round(pos)

    def do_events(self):
        for event in pygame.event.get():
            if event.type == pygame.QUIT:
                sys.exit()
            elif event.type == pygame.KEYDOWN:
                print(event)
            elif event.type == pygame.MOUSEBUTTONDOWN:
                print(event)
                hex = self.get_hex_at(event.pos)
                print(hex)
                if hex in self.grid:
                    del self.grid[hex]
                self.grid[hex] = self.renderer.instantiate(
                    Sector, hex_images[Images.Stones], self.layout, hex)
            elif event.type == pygame.MOUSEBUTTONUP:
                print(event)

    def loop(self):
        while True:
            self.do_events()
            self.renderer.perform_render()


if __name__ == "__main__":
    App()
