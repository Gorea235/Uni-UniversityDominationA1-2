# a bunch of this code was pulled from a small example file
# I made a while ago
# although, I have edited it very heavily
import pygame


class Camera():
    def __init__(self, x=0, y=0):
        self.x = x
        self.y = y

    def world_to_screen(self, x, y):
        return x - self.x, y - self.y

    def world_to_screen_rect(self, rect):
        x, y = self.world_to_screen(rect.x, rect.y)
        return pygame.Rect(x, y, rect.w, rect.h)

    def screen_to_world(self, x, y):
        return x + self.x, y + self.y

    def screen_to_world_rect(self, rect):
        x, y = self.screen_to_world(rect.x, rect.y)
        return pygame.Rect(x, y, rect.w, rect.h)

    @property
    def x(self):
        return self.__x

    @x.setter
    def x(self, value):
        self.__x = value

    @property
    def y(self):
        return self.__y

    @y.setter
    def y(self, value):
        self.__y = value


class Renderer():
    def __init__(self, screen, bg=(0, 0, 0)):
        self.__screen = screen
        self.__camera = Camera()
        self.bg = bg
        self.__render_objects = []

    def instantiate(self, obj, *args, **kwargs):
        nobj = obj(*args, **kwargs)
        self.__render_objects.append(nobj)
        return nobj

    def perform_render(self):
        self.__screen.fill(self.bg)
        for obj in self.__render_objects:
            self.__screen.blit(
                obj.surface, self.__camera.world_to_screen_rect(obj.rect))
        pygame.display.flip()

    @property
    def camera(self):
        return self.__camera


class RenderableObject():
    def __init__(self, surface):
        self.surface = surface
        self.rect = surface.get_rect()

    def move(self, x, y):
        self.rect = self.rect.move(x, y)

    @property
    def x(self):
        return self.rect.x

    @x.setter
    def x(self, value):
        self.rect.x = value

    @property
    def y(self):
        return self.rect.y

    @y.setter
    def y(self, value):
        self.rect.y = value

    @property
    def cx(self):
        return self.rect.centerx

    @cx.setter
    def cx(self, value):
        self.rect.centerx = value

    @property
    def cy(self):
        return self.rect.centery

    @cy.setter
    def cy(self, value):
        self.rect.centery = value

    @property
    def w(self):
        return self.rect.w

    @w.setter
    def w(self, value):
        self.rect.w = value

    @property
    def h(self):
        return self.rect.h

    @h.setter
    def h(self, value):
        self.rect.h = value

    @property
    def size(self):
        return self.rect.size

    @size.setter
    def size(self, value):
        self.rect.size = value

    @property
    def rect_details(self):
        return (self.x, self.y, self.w, self.h)

    @rect_details.setter
    def rect_details(self, value):
        self.x = value[0]
        self.y = value[1]
        self.w = value[2]
        self.h = value[3]


class RenderableImage(RenderableObject):
    def __init__(self, imgpath):
        super().__init__(pygame.image.load(imgpath).convert_alpha())

    def __scale_surface__(self):
        self.surface = pygame.transform.scale(self.surface, (self.w, self.h))

    @property
    def w(self):
        return RenderableObject.w.fget(self)

    @w.setter
    def w(self, value):
        RenderableObject.w.fset(self, value)
        self.__scale_surface__()

    @property
    def h(self):
        return RenderableObject.h.fget(self)

    @h.setter
    def h(self, value):
        RenderableObject.h.fset(self, value)
        self.__scale_surface__()

    @property
    def size(self):
        return RenderableObject.size.fget(self)

    @size.setter
    def size(self, value):
        RenderableObject.size.fset(self, value)
        self.__scale_surface__()

    @property
    def rect_details(self):
        return RenderableObject.rect_details.fget(self)

    @rect_details.setter
    def rect_details(self, value):
        RenderableObject.rect_details.fset(self, value)
        self.__scale_surface__()
