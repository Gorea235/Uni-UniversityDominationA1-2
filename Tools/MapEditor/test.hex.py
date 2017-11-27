# def test_hex_arithmetic():
#     equal_hex("hex_add", Hex(4, -10, 6), hex_add(Hex(1, -3, 2), Hex(3, -7, 4)))
#     equal_hex("hex_subtract", Hex(-2, 4, -2),
#               hex_subtract(Hex(1, -3, 2), Hex(3, -7, 4)))


# def test_hex_direction():
#     equal_hex("hex_direction", Hex(0, -1, 1), hex_direction(2))


# def test_hex_neighbor():
#     equal_hex("hex_neighbor", Hex(1, -3, 2), hex_neighbor(Hex(1, -2, 1), 2))


# def test_hex_diagonal():
#     equal_hex("hex_diagonal", Hex(-1, -1, 2),
#               hex_diagonal_neighbor(Hex(1, -2, 1), 3))


# def test_hex_distance():
#     equal_int("hex_distance", 7, hex_distance(Hex(3, -7, 4), Hex(0, 0, 0)))


# def test_hex_rotate_right():
#     equal_hex("hex_rotate_right", hex_rotate_right(
#         Hex(1, -3, 2)), Hex(3, -2, -1))


# def test_hex_rotate_left():
#     equal_hex("hex_rotate_left", hex_rotate_left(
#         Hex(1, -3, 2)), Hex(-2, -1, 3))


# def test_hex_round():
#     a = Hex(0, 0, 0)
#     b = Hex(1, -1, 0)
#     c = Hex(0, -1, 1)
#     equal_hex("hex_round 1", Hex(5, -10, 5),
#               hex_round(hex_lerp(Hex(0, 0, 0), Hex(10, -20, 10), 0.5)))
#     equal_hex("hex_round 2", hex_round(a), hex_round(hex_lerp(a, b, 0.499)))
#     equal_hex("hex_round 3", hex_round(b), hex_round(hex_lerp(a, b, 0.501)))
#     equal_hex("hex_round 4", hex_round(a), hex_round(Hex(a.q * 0.4 + b.q * 0.3 + c.q *
#                                                          0.3, a.r * 0.4 + b.r * 0.3 + c.r * 0.3, a.s * 0.4 + b.s * 0.3 + c.s * 0.3)))
#     equal_hex("hex_round 5", hex_round(c), hex_round(Hex(a.q * 0.3 + b.q * 0.3 + c.q *
#                                                          0.4, a.r * 0.3 + b.r * 0.3 + c.r * 0.4, a.s * 0.3 + b.s * 0.3 + c.s * 0.4)))


# def test_hex_linedraw():
#     equal_hex_array("hex_linedraw", [Hex(0, 0, 0), Hex(0, -1, 1), Hex(0, -2, 2), Hex(
#         1, -3, 2), Hex(1, -4, 3), Hex(1, -5, 4)], hex_linedraw(Hex(0, 0, 0), Hex(1, -5, 4)))


# def test_layout():
#     h = Hex(3, 4, -7)
#     flat = Layout(layout_flat, Point(10, 15), Point(35, 71))
#     equal_hex("layout", h, hex_round(
#         pixel_to_hex(flat, hex_to_pixel(flat, h))))
#     pointy = Layout(layout_pointy, Point(10, 15), Point(35, 71))
#     equal_hex("layout", h, hex_round(
#         pixel_to_hex(pointy, hex_to_pixel(pointy, h))))
