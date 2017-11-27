#! /usr/bin/env python3
import sys
from enum import Enum
import collections
import math


class Direction(Enum):
    NorthEast = 1
    East = 2
    SouthEast = 3
    SouthWest = 4
    West = 5
    NorthWest = 6


class DiagonalDirection(Enum):
    North = 1
    NorthEast = 2
    SouthEast = 3
    South = 4
    SouthWest = 5
    NorthWest = 6


Point = collections.namedtuple("Point", ["x", "y"])


class Coord:
    def __init__(self, q, r, s=None):
        if s is not None and abs(round(q + r + s, 14)) > sys.float_info.epsilon:
            raise ValueError("q + r + s must equal 0")
        self.q = q
        self.r = r
        self.s = s if s is not None else -q - r

    def __add__(self, other):
        return Coord(self.q + other.q, self.r + other.r)

    def __sub__(self, other):
        return Coord(self.q - other.q, self.r - other.r)

    def __mul__(self, other):
        if isinstance(other, int) or isinstance(other, float):
            return Coord(self.q * other, self.r * other)
        else:
            raise TypeError("Can only scale by int or float")

    def __len__(self):
        return (abs(self.q) + abs(self.r) + abs(self.s)) // 2

    def __round__(self):
        q = round(self.q)
        r = round(self.r)
        s = round(self.s)
        q_diff = abs(q - self.q)
        r_diff = abs(r - self.r)
        s_diff = abs(s - self.s)
        if q_diff > r_diff and q_diff > s_diff:
            q = -r - s
        else:
            if r_diff > s_diff:
                r = -q - s
            else:
                s = -q - r
        return Coord(q, r, s)

    def rotate_left(self):
        return Coord(-self.s, -self.q)

    def rotate_right(self):
        return Coord(-self.r, -self.s)

    def neighbor(self, direction):
        return self + coord_directions[direction]

    def diagonal(self, direction):
        return self + coord_diagonal_directions[direction]

    def distance_to(self, other):
        return len(self - other)

    @staticmethod
    def do_lerp(a, b, t):
        return a * (1 - t) + b * t

    def lerp(self, other, t):
        return Coord(Coord.do_lerp(self.q, other.q, t),
                     Coord.do_lerp(self.r, other.r, t),
                     Coord.do_lerp(self.s, other.s, t))

    def linedraw(self, other):
        N = self.distance_to(other)
        a_nudge = Coord(self.q + 0.000001, self.r +
                        0.000001, self.s - 0.000002)
        b_nudge = Coord(other.q + 0.000001, other.r +
                        0.000001, other.s - 0.000002)
        results = []
        step = 1.0 / max(N, 1)
        for i in range(0, N + 1):
            results.append(round(a_nudge.lerp(b_nudge, step * i)))
        return results


coord_directions = {
    Direction.NorthEast: Coord(1, 0, -1),
    Direction.East: Coord(1, -1, 0),
    Direction.SouthEast: Coord(0, -1, 1),
    Direction.SouthWest: Coord(-1, 0, 1),
    Direction.West: Coord(-1, 1, 0),
    Direction.NorthWest: Coord(0, 1, -1)
}

coord_diagonal_directions = {
    DiagonalDirection.North: Coord(1, 1, -2),
    DiagonalDirection.NorthEast: Coord(2, -1, -1),
    DiagonalDirection.SouthEast: Coord(1, -2, 1),
    DiagonalDirection.South: Coord(-1, -1, 2),
    DiagonalDirection.SouthWest: Coord(-2, 1, 1),
    DiagonalDirection.NorthWest: Coord(-1, 2, -1)
}

Orientation = collections.namedtuple("Orientation",
                                     [
                                         "f0", "f1", "f2", "f3",
                                         "b0", "b1", "b2", "b3",
                                         "start_angle"
                                     ])

orientation_pointy = Orientation(math.sqrt(3.0),
                                 math.sqrt(3.0) / 2.0,
                                 0.0,
                                 3.0 / 2.0,
                                 math.sqrt(3.0) / 3.0,
                                 -1.0 / 3.0,
                                 0.0,
                                 2.0 / 3.0,
                                 0.5)


class Layout:
    def __init__(self, orientation, size, origin):
        self.orientation = orientation
        self.size = size
        self.origin = origin

    def hex_to_pixel(self, h):
        M = self.orientation
        x = (M.f0 * h.q + M.f1 * h.r) * self.size.x
        y = (M.f2 * h.q + M.f3 * h.r) * self.size.y
        return Point(x + self.origin.x, y + self.origin.y)

    def pixel_to_hex(self, p):
        M = self.orientation
        pt = Point((p.x - self.origin.x) / self.size.x,
                   (p.y - self.origin.y) / self.size.y)
        q = M.b0 * pt.x + M.b1 * pt.y
        r = M.b2 * pt.x + M.b3 * pt.y
        return Coord(q, r)

    def hex_corner_offset(self, corner):
        M = self.orientation
        angle = 2.0 * math.pi * (M.start_angle - corner) / 6
        return Point(self.size.x * math.cos(angle), self.size.y * math.sin(angle))

    def polygon_corners(self, h):
        corners = []
        center = self.hex_to_pixel(h)
        for i in range(0, 6):
            offset = self.hex_corner_offset(i)
            corners.append(Point(center.x + offset.x, center.y + offset.y))
        return corners
