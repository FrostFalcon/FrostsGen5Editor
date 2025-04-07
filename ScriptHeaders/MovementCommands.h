#define Movement int

#define LookUp 0x0
#define LookDown 0x1
#define LookLeft 0x2
#define LookRight 0x3

#define SlowestWalkUp 0x4
#define SlowestWalkDown 0x5
#define SlowestWalkLeft 0x6
#define SlowestWalkRight 0x7

#define SlowWalkUp 0x8
#define SlowWalkDown 0x9
#define SlowWalkLeft 0xa
#define SlowWalkRight 0xb

#define WalkUp 0xc
#define WalkDown 0xd
#define WalkLeft 0xe
#define WalkRight 0xf

#define FastWalkUp 0x10
#define FastWalkDown 0x11
#define FastWalkLeft 0x12
#define FastWalkRight 0x13

#define FastestWalkUp 0x14
#define FastestWalkDown 0x15
#define FastestWalkLeft 0x16
#define FastestWalkRight 0x17

#define SlowestTurnUp 0x18
#define SlowestTurnDown 0x19
#define SlowestTurnLeft 0x1a
#define SlowestTurnRight 0x1b

#define SlowTurnUp 0x1c
#define SlowTurnDown 0x1d
#define SlowTurnLeft 0x1e
#define SlowTurnRight 0x1f

#define TurnUp 0x20
#define TurnDown 0x21
#define TurnLeft 0x22
#define TurnRight 0x23

#define FastTurnUp 0x24
#define FastTurnDown 0x25
#define FastTurnLeft 0x26
#define FastTurnRight 0x27

#define FastestTurnUp 0x28
#define FastestTurnDown 0x29
#define FastestTurnLeft 0x2a
#define FastestTurnRight 0x2b

#define SlowHopUp 0x2c
#define SlowHopDown 0x2d
#define SlowHopLeft 0x2e
#define SlowHopRight 0x2f

#define HopUp 0x30
#define HopDown 0x31
#define HopLeft 0x32
#define HopRight 0x33

#define JumpUp1 0x34
#define JumpDown1 0x35
#define JumpLeft1 0x36
#define JumpRight1 0x37

#define JumpUp2 0x38
#define JumpDown2 0x39
#define JumpLeft2 0x3a
#define JumpRight2 0x3b

#define Wait1 0x3c
#define Wait2 0x3d
#define Wait4 0x3e
#define Wait8 0x3f
#define Wait15 0x40
#define Wait16 0x41
#define Wait32 0x42

#define WarpPadUp 0x43
#define WarpPadDown 0x44

#define Vanish 0x45
#define Reappear 0x46

#define LockDirection 0x47
#define UnlockDirection 0x48

#define PauseAnim 0x49
#define UnpauseAnim 0x4a

#define Exclaimation 0x4b
#define QuestionMark 0x9f
#define MusicNote 0xa0
#define Ellipses 0xa1

#define MediumFastWalkUp 0x4c
#define MediumFastWalkDown 0x4d
#define MediumFastWalkLeft 0x4e
#define MediumFastWalkRight 0x4f

#define FasterWalkUp 0x50
#define FasterWalkDown 0x51
#define FasterWalkLeft 0x52
#define FasterWalkRight 0x53

#define InstantWalkUp 0x54
#define InstantWalkDown 0x55
#define InstantWalkLeft 0x56
#define InstantWalkRight 0x57

#define RunUp 0x58
#define RunDown 0x59
#define RunLeft 0x5a
#define RunRight 0x5b

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