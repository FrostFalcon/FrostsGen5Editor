#define Movement int

/*
0	Face Up	Face direction.Simple
1	Face Down
2	Face Left
3	Face Right
4	SlowWalk Up	REALLY SLOW
5	SlowWalk Down
6	SlowWalk Left
7	SlowWalk Right
8	Stroll Up	Slow Speed
9	Stroll Down
000A	Stroll Left
000B	Stroll Right
000C	Walk Up	Walk Speed
000D	Walk Down
000E	Walk Left
000F	Walk Right
10	Jog Up	Run Speed
11	Jog Down
12	Jog Left
13	Jog Right
14	Bike Up	Bike Speed
15	Bike Down
16	Bike Left
17	Bike Right
18	DontMove Up	Really slow
19	DontMove Down
001A	DontMove Left
001B	DontMove Right
001C	NoMove2 Up	Slow
001D	NoMove2 Down
001E	NoMove2 Left
001F	NoMove2 Right
20	NoMove3 Up	Normal ?
21	NoMove3 Down
22	NoMove3 Left
23	NoMove3 Right
24	NoMove4 Up	Pretty fast.
25	NoMove4 Down
26	NoMove4 Left
27	NoMove4 Right
28	NoMove5 Up	Really fast for turning
29	NoMove5 Down
002A	NoMove5 Left
002B	NoMove5 Right
002C	HopPlace1 Up	Kinda Slow
002D	HopPlace1 Down
002E	HopPlace1 Left
002F	HopPlace1 Right
30	HopPlace2 Up	Regular speed
31	HopPlace2 Down
32	HopPlace2 Left
33	HopPlace2 Right
34	HopMove Up	Kinda like acro bike
35	HopMove Down
36	HopMove Left
37	HopMove Right
38	HopBound Up	Leaps 2 * param in direction
39	HopBound Down
003A	HopBound Left
003B	HopBound Right
003C--
003D--
003E--
003F--
40	--
41	--
42	--
43	WarpUp	Straight into sky.
44	WarpDown	Straight down from sky.
45	Vanish	Nothing special.
46	Reappear
47	Lock Direction
48	Unlock Direction
49	--
004A--
004B	Exclaim	!
004C	Move Up	Walking speed.
004D	Move Down
004E	Move Left
004F	Move Right
50	Move2 Up	Bike Speed
51	Move2 Down
52	Move2 Left
53	Move2 Right
54	Move3 Up	Blazing fast.
55	Move3 Down
56	Move3 Left
57	Move3 Right
58	Spin Up	Walking speed.
59	Spin Down
005A	Spin Left
005B	Spin Right
005C	Pounce Left	medium speed hop
005D	Pounce Right
005E	Leap Left	2 panels
005F	Leap Right
60	Walk Up	Walk speed movement
61	Walk Down
62	Walk Left
63	Walk Right
64	Turn - 90 Return - 90 turn, +90 turn
65	HopInPlace	..
66	Bounce	Bounce once
67	Exclaim	!
68	MoveInPlace
69	RiseUp	Upwards * param
006A	RiseDown	Downwards * param
006B	90walk * param
006C	90 + up ?
006D	FRightRiseUp
006E	FLeftRiseDown
006F	Walk Up	..
70	Walk Down
71	Walk Down
72	Walk Up
73	SlideMove Left	dont face, just moonwalk!
74	SlideMove Right
75	Leap Up	Moon bounce * 3param
76	Leap Down	3
77	Leap Left	3
78	Leap Right	3
79	T90FL - RU	This stuff below is stupid.
007A	Down of ^ .
007B	Slide Down, Face Down	.
007C	Slide Up, Face Up	.
007D	RiseUpLeftFaceRight	.
007E	RiseDownRightcrap	.
007F	SlideUpFaceUp	.
80	FU - SlideUp - FU	.
81	FU - SlideDown - FD	.
82	FU - SlideUp - FU	.
83	FU - SlideLeft - FR	.
84	FD - SlideRight - FL	.
85	FL - Hop - RiseUp	.
86	FR - Hop - RiseDown	.
87	TD - HopForward	.
88	TU - HopForward	.
89	TR - Hop - RiseUp	.
008A	TL - Hop - RiseDown	.
008B	Hop Up	.
008C	Hop Down * param
008D	Hop Down	.
008E	Hop Up	.
008F	HopSlide Left	.
90	HopSlide Right	.
91	FastWalkDown	.
92	FastWalkUp	.
93	Slide Left	.
94	Slide Right	.
95	RapidWalk Down	.
96	RapidWalk Up	.
97	QuickSlide Left	.
98	QuickSlide Right	.
99	Exclaim 	.
009A	Hopscotch in	.
009B	Shuffle Up
009C	Shuffle Down
009D	Shuffle Left
009E	Shuffle Right
009F	Question
00A0	MusicNote
00A1	"..."
*/