#! /usr/bin/env python3
import math
import os
import sys
import pygame

import game_lib
import hex

pygame.init()

WINDOW_TITLE = "weed simulator 2000"
SIZE = WIDTH, HEIGHT = 600, 600


class Sector(game_lib.RenderableImage):
    def __init__(self, imgpath, layout, pos=None):
        super().__init__(imgpath)
        self.__layout = layout
        self.pos = pos if pos is not None else hex.Coord(0, 0)

    @property
    def pos(self):
        return self.__pos

    @pos.setter
    def pos(self, value):
        self.__pos = value


class App():
    def __init__(self):
        self.screen = pygame.display.set_mode(SIZE)
        pygame.display.set_caption(WINDOW_TITLE)
        self.renderer = game_lib.Renderer(self.screen)
        self.layout = hex.Layout(
            hex.orientation_pointy, hex.Point(10, 10), hex.Point(250, 250))
        self.grid = {}
        if True: # todo: add sys.argv checking for loading json
            origin = hex.Coord(0, 0)
            self.grid[origin] = self.renderer.instantiate(
                Sector, "", self.layout, origin) # todo: setup hex images
        self.loop()

    def do_events(self):
        for event in pygame.event.get():
            if event.type == pygame.QUIT:
                sys.exit()
            elif event.type == pygame.KEYDOWN:
                pass

    def loop(self):
        while True:
            self.do_events()
            self.renderer.perform_render()


if __name__ == "__main__":
    App()
