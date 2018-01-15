# a bunch of this code was pulled from a small example file
# I made a while ago
# although, I have edited it very heavily
from enum import Enum
import pygame

DEFAULT_FONT_NAME = 'Arial'
DEFAULT_FONT_SIZE = 20


class RenderMode(Enum):
    World = 1
    Screen = 2


class Camera():
    def __init__(self, x=0, y=0):
        self.x = x
        self.y = y

    def world_to_screen(self, x, y):
        return x - self.x, y - self.y

    def world_to_screen_rect(self, rect):
        x, y = self.world_to_screen(rect.x, rect.y)
        nrect = rect.copy()
        nrect.x = x
        nrect.y = y
        return nrect

    def screen_to_world(self, x, y):
        return x + self.x, y + self.y

    def screen_to_world_rect(self, rect):
        x, y = self.screen_to_world(rect.x, rect.y)
        nrect = rect.copy()
        nrect.x = x
        nrect.y = y
        return nrect

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
        self.__render_objs_world = []
        self.__render_objs_screen = []

    def __init_obj__(self, obj, mode, collection, *args, **kwargs):
        nobj = obj(*args, **kwargs)
        assert isinstance(nobj, RenderableObject)  # ensure object correctness
        nobj._mode = mode
        collection.append(nobj)
        return nobj

    def init_world_obj(self, obj, *args, **kwargs):
        return self.__init_obj__(obj, RenderMode.World, self.__render_objs_world, *args, **kwargs)

    def init_screen_obj(self, obj, *args, **kwargs):
        return self.__init_obj__(obj, RenderMode.Screen, self.__render_objs_screen, *args, **kwargs)

    def remove_obj(self, obj):
        try:
            self.__render_objs_world.remove(obj)
        except ValueError:
            try:
                self.__render_objs_screen.remove(obj)
            except ValueError:
                raise ValueError("object given is not tracked by renderer")

    def __do_render_for__(self, collection):
        for obj in collection:
            if obj.active:
                obj.render(self.__screen, self.__camera)

    def perform_render(self):
        self.__screen.fill(self.bg)
        self.__do_render_for__(self.__render_objs_world)
        self.__do_render_for__(self.__render_objs_screen)
        pygame.display.flip()

    @property
    def camera(self):
        return self.__camera


class RenderableObject():
    _mode = None
    active = True

    def _get_draw_pos(self, x, y, camera):
        if self._mode == RenderMode.World:
            x, y = camera.world_to_screen(x, y)
        elif self._mode == RenderMode.Screen:
            pass  # we don't need to process a rect on the screen
        else:
            raise ValueError("Render mode {} is not valid".format(self._mode))
        return x, y

    def _get_draw_rect(self, rect, camera):
        x, y = self._get_draw_pos(rect.x, rect.y, camera)
        nrect = rect.copy()
        nrect.x = x
        nrect.y = y
        return nrect

    def render(self, screen, camera):
        raise NotImplementedError("on object {}".format(self))


class RectObject():
    # lists of callable objects that are fired when the
    # rect object changes position or shape
    _change_pos = []
    _change_size = []

    _rect = None

    def __change_pos_event__(self):
        for c in self._change_pos:
            c()

    def __change_size_event__(self):
        for c in self._change_size:
            c()

    @property
    def x(self):
        return self._rect.x

    @x.setter
    def x(self, value):
        self._rect.x = value
        self.__change_pos_event__()

    @property
    def y(self):
        return self._rect.y

    @y.setter
    def y(self, value):
        self._rect.y = value
        self.__change_pos_event__()

    @property
    def l(self):
        return self._rect.left

    @l.setter
    def l(self, value):
        self._rect.left = value
        self.__change_pos_event__()

    @property
    def t(self):
        return self._rect.top

    @t.setter
    def t(self, value):
        self._rect.top = value
        self.__change_pos_event__()

    @property
    def r(self):
        return self._rect.right

    @r.setter
    def r(self, value):
        self._rect.right = value
        self.__change_pos_event__()

    @property
    def b(self):
        return self._rect.bottom

    @b.setter
    def b(self, value):
        self._rect.bottom = value
        self.__change_pos_event__()

    @property
    def tl(self):
        return self._rect.topleft

    @tl.setter
    def tl(self, value):
        self._rect.topleft = value
        self.__change_pos_event__()

    @property
    def tr(self):
        return self._rect.topright

    @tr.setter
    def tr(self, value):
        self._rect.topright = value
        self.__change_pos_event__()

    @property
    def bl(self):
        return self._rect.bottomleft

    @bl.setter
    def bl(self, value):
        self._rect.bottomleft = value
        self.__change_pos_event__()

    @property
    def br(self):
        return self._rect.br

    @br.setter
    def br(self, value):
        self._rect.bottomright = value
        self.__change_pos_event__()

    @property
    def cx(self):
        return self._rect.centerx

    @cx.setter
    def cx(self, value):
        self._rect.centerx = value
        self.__change_pos_event__()

    @property
    def cy(self):
        return self._rect.centery

    @cy.setter
    def cy(self, value):
        self._rect.centery = value
        self.__change_pos_event__()

    @property
    def c(self):
        return self._rect.center

    @c.setter
    def c(self, value):
        self._rect.center = value
        self.__change_pos_event__()

    @property
    def w(self):
        return self._rect.w

    @w.setter
    def w(self, value):
        self._rect.w = value
        self.__change_size_event__()

    @property
    def h(self):
        return self._rect.h

    @h.setter
    def h(self, value):
        self._rect.h = value
        self.__change_size_event__()

    @property
    def s(self):
        return self._rect.size

    @s.setter
    def s(self, value):
        self._rect.size = value
        self.__change_size_event__()

    @property
    def rect_details(self):
        return (self.x, self.y, self.w, self.h)

    @rect_details.setter
    def rect_details(self, value):
        self.x = value[0]
        self.y = value[1]
        self.w = value[2]
        self.h = value[3]
        self.__change_pos_event__()
        self.__change_size_event__()


class RenderableSurface(RenderableObject, RectObject):
    def __init__(self, surface):
        self._surface = surface
        self._rect = surface.get_rect()
        self._change_size.append(self.__scale_surface__)

    def render(self, screen, camera):
        screen.blit(self._surface, self._get_draw_rect(self._rect, camera))

    def move(self, x, y):
        self._rect = self._rect.move(x, y)

    def __scale_surface__(self):
        self._surface = pygame.transform.scale(self._surface, (self.w, self.h))


class RenderableImage(RenderableSurface):
    def __init__(self, imgpath):
        super().__init__(self.__get_img_surface__(imgpath))

    def __get_img_surface__(self, imgpath):
        return pygame.image.load(imgpath).convert_alpha()

    def set_image(self, imgpath):
        self._surface = self.__get_img_surface__(imgpath)
        self.s = self.s  # cause a rescale of the surface


class RenderableText(RenderableObject):
    def __init__(self, text, fg=(255, 255, 255), bg=None, antialias=True, font=None):
        self.__text = text
        self.__antialias = antialias
        self.__fg = fg
        self.__bg = bg
        self.__font = font if font is not None else pygame.font.SysFont(
            DEFAULT_FONT_NAME, DEFAULT_FONT_SIZE)
        # using rect to keep track of positioning vars
        self.__rect = pygame.rect.Rect(0, 0, 1, 1)
        self.__render_text__()

    def render(self, screen, camera):
        screen.blit(self.__surface, (self.x, self.y))

    def __render_text__(self):
        self.__surface = self.__font.render(
            self.text, self.antialias, self.fg, self.bg)
        trect = self.__surface.get_rect()
        trect.x = self.x
        trect.y = self.y
        self.__rect = trect

    @property
    def x(self):
        return self.__rect.x

    @x.setter
    def x(self, value):
        self.__rect.x = value

    @property
    def y(self):
        return self.__rect.y

    @y.setter
    def y(self, value):
        self.__rect.y = value

    @property
    def cx(self):
        return self.__rect.centerx

    @cx.setter
    def cx(self, value):
        self.__rect.centerx = value

    @property
    def cy(self):
        return self.__rect.centery

    @cy.setter
    def cy(self, value):
        self.__rect.centery = value

    @property
    def top(self):
        return self.__rect.top

    @top.setter
    def top(self, value):
        self.__rect.top = value

    @property
    def left(self):
        return self.__rect.left

    @left.setter
    def left(self, value):
        self.__rect.left = value

    @property
    def text(self):
        return self.__text

    @text.setter
    def text(self, value):
        self.__text = value
        self.__render_text__()

    @property
    def antialias(self):
        return self.__antialias

    @antialias.setter
    def antialias(self, value):
        self.__antialias = value
        self.__render_text__()

    @property
    def fg(self):
        return self.__fg

    @fg.setter
    def fg(self, value):
        self.__fg = value
        self.__render_text__()

    @property
    def bg(self):
        return self.__bg

    @bg.setter
    def bg(self, value):
        self.__bg = value
        self.__render_text__()


class RenderableRect(RenderableObject, RectObject):
    def __init__(self, color, border_color=None, border_width=0):
        self.color = color
        self.border_color = border_color
        self.border_width = border_width
        self._rect = pygame.rect.Rect(0, 0, 10, 10)

    def render(self, screen, camera):
        pygame.draw.rect(screen, self.color,
                         self._get_draw_rect(self._rect, camera))
        if self.border_color is not None and self.border_width > 0:
            pygame.draw.lines(screen, self.border_color, True, [
                self._get_draw_pos(self.x, self.y, camera),
                self._get_draw_pos(self.x + self.w, self.y, camera),
                self._get_draw_pos(self.x + self.w, self.y + self.h, camera),
                self._get_draw_pos(self.x, self.y + self.h, camera)
            ], self.border_width)
