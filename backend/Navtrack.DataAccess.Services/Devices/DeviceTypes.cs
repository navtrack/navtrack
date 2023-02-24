namespace Navtrack.DataAccess.Services.Devices;

public static class DeviceTypes
{
    public const string DeviceTypesCsv = @"
1;Meitrack;MT80A;meitrack;7001
2;Meitrack;MT80i;meitrack;7001
3;Meitrack;MT88;meitrack;7001
4;Meitrack;MT90;meitrack;7001
5;Meitrack;MT90G;meitrack;7001
6;Meitrack;MT90L;meitrack;7001
7;Meitrack;MT90V4;meitrack;7001
8;Meitrack;MVT100;meitrack;7001
9;Meitrack;MVT340;meitrack;7001
10;Meitrack;MVT380;meitrack;7001
11;Meitrack;MVT600;meitrack;7001
12;Meitrack;MVT800;meitrack;7001
13;Meitrack;T1;meitrack;7001
14;Meitrack;T3;meitrack;7001
15;Meitrack;T311;meitrack;7001
16;Meitrack;T322X;meitrack;7001
17;Meitrack;T333;meitrack;7001
18;Meitrack;T355;meitrack;7001
19;Meitrack;T366;meitrack;7001
20;Meitrack;T366G;meitrack;7001
21;Meitrack;T366L;meitrack;7001
22;Meitrack;TC68S;meitrack;7001
23;Teltonika;AT1000;teltonika;7002
24;Teltonika;AT2000;teltonika;7002
25;Teltonika;CT2000E;teltonika;7002
26;Teltonika;FM1000;teltonika;7002
27;Teltonika;FM1010;teltonika;7002
28;Teltonika;FM1100;teltonika;7002
29;Teltonika;FM1110;teltonika;7002
30;Teltonika;FM1120;teltonika;7002
31;Teltonika;FM1122;teltonika;7002
32;Teltonika;FM1125;teltonika;7002
33;Teltonika;FM1200;teltonika;7002
34;Teltonika;FM1202;teltonika;7002
35;Teltonika;FM1204;teltonika;7002
36;Teltonika;FM2100;teltonika;7002
37;Teltonika;FM2200;teltonika;7002
38;Teltonika;FM3001;teltonika;7002
39;Teltonika;FM3101;teltonika;7002
40;Teltonika;FM3200;teltonika;7002
41;Teltonika;FM3300;teltonika;7002
42;Teltonika;FM3400;teltonika;7002
43;Teltonika;FM3600;teltonika;7002
44;Teltonika;FM3612;teltonika;7002
45;Teltonika;FM3620;teltonika;7002
46;Teltonika;FM3622;teltonika;7002
47;Teltonika;FM36M1;teltonika;7002
48;Teltonika;FM4100;teltonika;7002
49;Teltonika;FM4200;teltonika;7002
50;Teltonika;FM5300;teltonika;7002
51;Teltonika;FM5500;teltonika;7002
52;Teltonika;FM6300;teltonika;7002
53;Teltonika;FM6320;teltonika;7002
54;Teltonika;FMA110;teltonika;7002
55;Teltonika;FMA120;teltonika;7002
56;Teltonika;FMA202;teltonika;7002
57;Teltonika;FMA204;teltonika;7002
58;Teltonika;FMB001;teltonika;7002
59;Teltonika;FMB010;teltonika;7002
60;Teltonika;FMB110;teltonika;7002
61;Teltonika;FMB120;teltonika;7002
62;Teltonika;FMB122;teltonika;7002
63;Teltonika;FMB125;teltonika;7002
64;Teltonika;FMB130;teltonika;7002
65;Teltonika;FMB140;teltonika;7002
66;Teltonika;FMB202;teltonika;7002
67;Teltonika;FMB204;teltonika;7002
68;Teltonika;FMB207;teltonika;7002
69;Teltonika;FMB208;teltonika;7002
70;Teltonika;FMB630;teltonika;7002
71;Teltonika;FMB640;teltonika;7002
72;Teltonika;FMB900;teltonika;7002
73;Teltonika;FMB920;teltonika;7002
74;Teltonika;FMB962;teltonika;7002
75;Teltonika;FMB964;teltonika;7002
76;Teltonika;FMC001;teltonika;7002
77;Teltonika;FMC125;teltonika;7002
78;Teltonika;FMC130;teltonika;7002
79;Teltonika;FMC640;teltonika;7002
80;Teltonika;FMM001;teltonika;7002
81;Teltonika;FMM125;teltonika;7002
82;Teltonika;FMM130;teltonika;7002
83;Teltonika;FMM640;teltonika;7002
84;Teltonika;FMT100;teltonika;7002
85;Teltonika;FMU125;teltonika;7002
86;Teltonika;FMU130;teltonika;7002
87;Teltonika;GH1201;teltonika;7002
88;Teltonika;GH3000;teltonika;7002
89;Teltonika;GH4000;teltonika;7002
90;Teltonika;GH5200;teltonika;7002
91;Teltonika;MH2000;teltonika;7002
92;Teltonika;MSP500;teltonika;7002
93;Teltonika;MTB100;teltonika;7002
94;Teltonika;RUT850;teltonika;7002
95;Teltonika;RUT955;teltonika;7002
96;Teltonika;RUTX11;teltonika;7002
97;Teltonika;TMT250;teltonika;7002
98;Teltonika;TST100;teltonika;7002
99;Meiligao;GT30i;meiligao;7003
100;Meiligao;GT60;meiligao;7003
101;Meiligao;VT300;meiligao;7003
102;Meiligao;VT310;meiligao;7003
103;Meiligao;VT400;meiligao;7003
104;Megastek Technologies;FMS-110;megastek;7004
105;Megastek Technologies;FMS-120;megastek;7004
106;Megastek Technologies;GVT-430;megastek;7004
107;Megastek Technologies;GVT-800;megastek;7004
108;Megastek Technologies;GVT-900;megastek;7004
109;Megastek Technologies;MT-100;megastek;7004
110;Megastek Technologies;MT-110;megastek;7004
111;Megastek Technologies;MT-200x;megastek;7004
112;Megastek Technologies;MT-60;megastek;7004
113;Megastek Technologies;MT-60X;megastek;7004
114;Megastek Technologies;MT-65;megastek;7004
115;Megastek Technologies;MT-70;megastek;7004
116;Megastek Technologies;MT-90N;megastek;7004
117;Megastek Technologies;XT-007;megastek;7004
118;Megastek Technologies;XT-007G;megastek;7004
119;Megastek Technologies;XT-007W;megastek;7004
120;TotemTech;AT02;totem;7005
121;TotemTech;AT03;totem;7005
122;TotemTech;AT04;totem;7005
123;TotemTech;AT05;totem;7005
124;TotemTech;AT05-3G;totem;7005
125;TotemTech;AT06;totem;7005
126;TotemTech;AT07;totem;7005
127;TotemTech;AT07-3G;totem;7005
128;TotemTech;AT09;totem;7005
129;TotemTech;AT09-3G;totem;7005
130;TZONE Digital Technology;TZ-AVL02;tzone;7006
131;TZONE Digital Technology;TZ-AVL03;tzone;7006
132;TZONE Digital Technology;TZ-AVL05;tzone;7006
133;TZONE Digital Technology;TZ-AVL08;tzone;7006
134;TZONE Digital Technology;TZ-AVL09;tzone;7006
135;TZONE Digital Technology;TZ-AVL10;tzone;7006
136;TZONE Digital Technology;TZ-AVL19;tzone;7006
137;TZONE Digital Technology;TZ-AVL201;tzone;7006
138;TZONE Digital Technology;TZ-AVL301;tzone;7006
139;TZONE Digital Technology;TZ-GT01;tzone;7006
140;TZONE Digital Technology;TZ-GT08;tzone;7006
141;TZONE Digital Technology;TZ-GT09;tzone;7006
142;TZONE Digital Technology;TZ-VN06;tzone;7006
143;Coban Electronics;GPS-102B;coban;7007
144;Coban Electronics;GPS-103A;coban;7007
145;Coban Electronics;GPS-103B;coban;7007
146;Coban Electronics;GPS-104;coban;7007
147;Coban Electronics;GPS-105A;coban;7007
148;Coban Electronics;GPS-105B;coban;7007
149;Coban Electronics;GPS-108;coban;7007
150;Coban Electronics;GPS-303F;coban;7007
151;Coban Electronics;GPS-303G;coban;7007
152;Coban Electronics;GPS-303H;coban;7007
153;Coban Electronics;GPS-303I;coban;7007
154;Coban Electronics;GPS-306;coban;7007
155;Coban Electronics;GPS-308;coban;7007
156;Coban Electronics;GPS-310;coban;7007
157;Coban Electronics;GPS-311;coban;7007
158;Queclink;GB100;queclink;7008
159;Queclink;GL100;queclink;7008
160;Queclink;GL100M;queclink;7008
161;Queclink;GL300;queclink;7008
162;Queclink;GL300A;queclink;7008
163;Queclink;GL300W;queclink;7008
164;Queclink;GL500;queclink;7008
165;Queclink;GL501MG;queclink;7008
166;Queclink;GL505;queclink;7008
167;Queclink;GL530;queclink;7008
168;Queclink;GMT100;queclink;7008
169;Queclink;GMT200;queclink;7008
170;Queclink;GT301;queclink;7008
171;Queclink;GV300;queclink;7008
172;Queclink;GV300CAN;queclink;7008
173;Queclink;GV350;queclink;7008
174;Queclink;GV50;queclink;7008
175;Queclink;GV500;queclink;7008
176;Queclink;GV500MA;queclink;7008
177;Queclink;GV500MAP;queclink;7008
178;Queclink;GV55;queclink;7008
179;Queclink;GV56;queclink;7008
180;Queclink;GV56RS;queclink;7008
181;Queclink;GV600;queclink;7008
182;Queclink;GV65;queclink;7008
183;Queclink;GV75;queclink;7008
184;Queclink;GV800G;queclink;7008
185;fifotrack;A01;fifotrack;7009
186;fifotrack;A100;fifotrack;7009
187;fifotrack;A300;fifotrack;7009
188;fifotrack;A500;fifotrack;7009
189;fifotrack;A600;fifotrack;7009
190;fifotrack;A700;fifotrack;7009
191;fifotrack;Q1;fifotrack;7009
192;fifotrack;S20;fifotrack;7009
193;fifotrack;S30;fifotrack;7009
194;fifotrack;S50;fifotrack;7009
195;fifotrack;S60;fifotrack;7009
196;Suntech;ST310U;suntech;7010
197;Suntech;ST330;suntech;7010
198;Suntech;ST330;suntech;7010
199;Suntech;ST3300;suntech;7010
200;Suntech;ST3310;suntech;7010
201;Suntech;ST3330;suntech;7010
202;Suntech;ST3330;suntech;7010
203;Suntech;ST3340;suntech;7010
204;Suntech;ST340;suntech;7010
205;Suntech;ST3940;suntech;7010
206;Suntech;ST4300;suntech;7010
207;Suntech;ST4310;suntech;7010
208;Suntech;ST4330;suntech;7010
209;Suntech;ST4330;suntech;7010
210;Suntech;ST4340;suntech;7010
211;Suntech;ST4910;suntech;7010
212;Suntech;ST4940;suntech;7010
213;Suntech;ST940;suntech;7010
214;N/A;AT-1;tkstar;7011
215;N/A;AT-18;tkstar;7011
216;N/A;G91S;tkstar;7011
217;N/A;GT005;tkstar;7011
218;N/A;GTR-100;tkstar;7011
219;Gator;GTRACK4G;tkstar;7011
220;N/A;H-02A;tkstar;7011
221;N/A;H-02B;tkstar;7011
222;N/A;H-06;tkstar;7011
223;N/A;H02;tkstar;7011
224;N/A;H08;tkstar;7011
225;Amparos;M2;tkstar;7011
226;Amparos;M5;tkstar;7011
227;N/A;MI-G6;tkstar;7011
228;N/A;Mini GPS Tracker;tkstar;7011
229;N/A;NT19A;tkstar;7011
230;N/A;NT201;tkstar;7011
231;N/A;NT202;tkstar;7011
232;N/A;NT21S;tkstar;7011
233;N/A;PT301;tkstar;7011
234;N/A;Realtime OBD Car GPS Tracker;tkstar;7011
235;ReachFar;RF-16V;tkstar;7011
236;N/A;S31;tkstar;7011
237;Amparos;S4;tkstar;7011
238;Amparos;S6;tkstar;7011
239;Amparos;S71;tkstar;7011
240;Amparos;S9;tkstar;7011
241;N/A;TK110;tkstar;7011
242;N/A;TX-2;tkstar;7011
243;SinoTrack;ST-901;sinotrack;7012
244;SinoTrack;ST-902;sinotrack;7012
245;SinoTrack;ST-903;sinotrack;7012
246;SinoTrack;ST-904;sinotrack;7012
247;SinoTrack;ST-905;sinotrack;7012
248;SinoTrack;ST-908;sinotrack;7012
249;SinoTrack;ST-915;sinotrack;7012
250;TKSTAR;Mini GPS Tracker;tkstar;7012
251;TKSTAR;Motorcycle GPS Tracker;tkstar;7012
252;TKSTAR;OBD;tkstar;7012
253;TKSTAR;Power Tracker 4G;tkstar;7012
254;TKSTAR;Relay GPS Tracker;tkstar;7012
255;TKSTAR;TK105;tkstar;7012
256;TKSTAR;TK106;tkstar;7012
257;TKSTAR;TK106 3G;tkstar;7012
258;TKSTAR;TK109;tkstar;7012
259;TKSTAR;TK109 3G;tkstar;7012
260;TKSTAR;TK208;tkstar;7012
261;TKSTAR;TK209A;tkstar;7012
262;TKSTAR;TK209A 3G;tkstar;7012
263;TKSTAR;TK209A 4G;tkstar;7012
264;TKSTAR;TK209B;tkstar;7012
265;TKSTAR;TK209B 3G;tkstar;7012
266;TKSTAR;TK209C;tkstar;7012
267;TKSTAR;TK210;tkstar;7012
268;TKSTAR;TK210 3G;tkstar;7012
269;TKSTAR;TK210 4G;tkstar;7012
270;TKSTAR;TK210B;tkstar;7012
271;TKSTAR;TK300;tkstar;7012
272;TKSTAR;TK800;tkstar;7012
273;TKSTAR;TK909;tkstar;7012
274;Concox;AT1;concox;7013
275;Concox;AT2;concox;7013
276;Concox;AT3;concox;7013
277;Concox;AT4;concox;7013
278;Concox;AT6;concox;7013
279;Concox;BL10;concox;7013
280;Concox;CT10;concox;7013
281;Concox;EG02;concox;7013
282;Concox;ET25;concox;7013
283;Concox;GK310;concox;7013
284;Concox;GT06;concox;7013
285;Concox;GT06E;concox;7013
286;Concox;GT06F;concox;7013
287;Concox;GT06N;concox;7013
288;Concox;GT07;concox;7013
289;Concox;GT08;concox;7013
290;Concox;GT800;concox;7013
291;Concox;GV20;concox;7013
292;Concox;GV40;concox;7013
293;Concox;HVT001;concox;7013
294;Concox;JM-LG01;concox;7013
295;Concox;JM-LG02;concox;7013
296;Concox;JM-LG05;concox;7013
297;Concox;JM-LL01;concox;7013
298;Concox;JM-VG01U;concox;7013
299;Concox;JM-VL02;concox;7013
300;Concox;OB22;concox;7013
301;Concox;Q2;concox;7013
302;Concox;Qbit;concox;7013
303;Concox;TR06;concox;7013
304;Concox;WeTrack Lite;concox;7013
305;Concox;WeTrack140;concox;7013
306;Concox;WeTrack2;concox;7013
307;Concox;X3;concox;7013
308;CanTrack;G01;cantrack;7014
309;CanTrack;G02;cantrack;7014
310;CanTrack;G02M;cantrack;7014
311;CanTrack;G03;cantrack;7014
312;CanTrack;G05;cantrack;7014
313;CanTrack;G200;cantrack;7014
314;CanTrack;G500;cantrack;7014
315;CanTrack;G900;cantrack;7014
316;CanTrack;GT02;cantrack;7014
317;CanTrack;TK06A;cantrack;7014
318;CanTrack;TK100;cantrack;7014
319;CanTrack;TK102B;cantrack;7014
320;CanTrack;TK103;cantrack;7014
321;CanTrack;TK103B;cantrack;7014
322;CanTrack;TK200A;cantrack;7014
323;LKGPS;330C;lkgps;7015
324;LKGPS;930A;lkgps;7015
325;LKGPS;LK106;lkgps;7015
326;LKGPS;LK106-3G;lkgps;7015
327;LKGPS;LK108;lkgps;7015
328;LKGPS;LK109;lkgps;7015
329;LKGPS;LK110;lkgps;7015
330;LKGPS;LK120;lkgps;7015
331;LKGPS;LK206;lkgps;7015
332;LKGPS;LK206-3G;lkgps;7015
333;LKGPS;LK206A;lkgps;7015
334;LKGPS;LK206B;lkgps;7015
335;LKGPS;LK208;lkgps;7015
336;LKGPS;LK209A;lkgps;7015
337;LKGPS;LK209B;lkgps;7015
338;LKGPS;LK209C;lkgps;7015
339;LKGPS;LK210;lkgps;7015
340;LKGPS;LK210-3G;lkgps;7015
341;LKGPS;LK300;lkgps;7015
342;LKGPS;LK310;lkgps;7015
343;LKGPS;LK330;lkgps;7015
344;LKGPS;LK330B;lkgps;7015
345;LKGPS;LK660;lkgps;7015
346;LKGPS;LK670;lkgps;7015
347;LKGPS;LK670B;lkgps;7015
348;LKGPS;LK680;lkgps;7015
349;LKGPS;LK700;lkgps;7015
350;LKGPS;LK710;lkgps;7015
351;LKGPS;LK710C;lkgps;7015
352;LKGPS;LK730;lkgps;7015
353;LKGPS;LK770;lkgps;7015
354;LKGPS;LK800;lkgps;7015
355;LKGPS;LK910;lkgps;7015
356;LKGPS;LK930B;lkgps;7015
357;LKGPS;LK930C;lkgps;7015
358;LKGPS;LK930D;lkgps;7015
359;Carscop;CCTR-600;carscop;7016
360;Carscop;CCTR-620;carscop;7016
361;Carscop;CCTR-620+;carscop;7016
362;Carscop;CCTR-622;carscop;7016
363;Carscop;CCTR-622+;carscop;7016
364;Carscop;CCTR-622G;carscop;7016
365;Carscop;CCTR-623;carscop;7016
366;Carscop;CCTR-626;carscop;7016
367;Carscop;CCTR-628;carscop;7016
368;Carscop;CCTR-629;carscop;7016
369;Carscop;CCTR-630;carscop;7016
370;Carscop;CCTR-631;carscop;7016
371;Carscop;CCTR-632;carscop;7016
372;Carscop;CCTR-800+;carscop;7016
373;Carscop;CCTR-800+V2;carscop;7016
374;Carscop;CCTR-800G;carscop;7016
375;Carscop;CCTR-800G-4G;carscop;7016
376;Carscop;CCTR-803;carscop;7016
377;Carscop;CCTR-803+;carscop;7016
378;Carscop;CCTR-803B;carscop;7016
379;Carscop;CCTR-803BT;carscop;7016
380;Carscop;CCTR-803G;carscop;7016
381;Carscop;CCTR-804;carscop;7016
382;Carscop;CCTR-805G-V2;carscop;7016
383;Carscop;CCTR-805G-V3;carscop;7016
384;Carscop;CCTR-806;carscop;7016
385;Carscop;CCTR-808S;carscop;7016
386;Carscop;CCTR-809;carscop;7016
387;Carscop;CCTR-809A;carscop;7016
388;Carscop;CCTR-809P;carscop;7016
389;Carscop;CCTR-811;carscop;7016
390;Carscop;CCTR-820;carscop;7016
391;Carscop;CCTR-821;carscop;7016
392;Carscop;CCTR-822;carscop;7016
393;Carscop;CCTR-822D;carscop;7016
394;Carscop;CCTR-823;carscop;7016
395;Carscop;CCTR-828;carscop;7016
396;Carscop;CCTR-829;carscop;7016
397;Carscop;CCTR-830;carscop;7016
398;Carscop;CCTR-830+;carscop;7016
399;Carscop;CCTR-830C;carscop;7016
400;Carscop;CCTR-830G;carscop;7016
401;Carscop;CCTR-831;carscop;7016
402;Carscop;CCTR-922;carscop;7016
403;Xexun;TK101;xexun;7017
404;Xexun;TK102;xexun;7017
405;Xexun;TK102-2;xexun;7017
406;Xexun;TK103;xexun;7017
407;Xexun;TK103-2;xexun;7017
408;Xexun;TK104;xexun;7017
409;Xexun;TK106;xexun;7017
410;Xexun;TK107;xexun;7017
411;Xexun;TK108;xexun;7017
412;Xexun;TK201;xexun;7017
413;Xexun;TK201-2;xexun;7017
414;Xexun;TK202;xexun;7017
415;Xexun;TK203;xexun;7017
416;Xexun;TK206;xexun;7017
417;Xexun;XT008;xexun;7017
418;Xexun;XT009;xexun;7017
419;Xexun;XT011;xexun;7017
420;Xexun;XT107;xexun;7017
421;iStartek;PT19;istartek;7018
422;iStartek;PT21;istartek;7018
423;iStartek;PT23;istartek;7018
424;iStartek;PT31;istartek;7018
425;iStartek;PT50;istartek;7018
426;iStartek;PT55;istartek;7018
427;iStartek;PT60;istartek;7018
428;iStartek;VT100;istartek;7018
429;iStartek;VT100;istartek;7018
430;iStartek;VT140;istartek;7018
431;iStartek;VT202;istartek;7018
432;iStartek;VT203;istartek;7018
433;iStartek;VT206;istartek;7018
434;iStartek;VT206;istartek;7018
435;iStartek;VT208;istartek;7018
436;iStartek;VT600;istartek;7018
437;iStartek;VT600;istartek;7018
438;iStartek;VT900;istartek;7018
439;iStartek;VT900-G;istartek;7018
440;iStartek;VT900-L;istartek;7018
441;XeElectech;GRTQ;xeelectech;7019
442;XeElectech;Mini GPS Tracker;xeelectech;7019
443;XeElectech;XE103;xeelectech;7019
444;XeElectech;XE106;xeelectech;7019
445;XeElectech;XE109;xeelectech;7019
446;XeElectech;XE120;xeelectech;7019
447;XeElectech;XE201;xeelectech;7019
448;XeElectech;XE208;xeelectech;7019
449;XeElectech;XE209A;xeelectech;7019
450;XeElectech;XE209B;xeelectech;7019
451;XeElectech;XE209C;xeelectech;7019
452;XeElectech;XE210;xeelectech;7019
453;XeElectech;XE210-3G;xeelectech;7019
454;XeElectech;XE300;xeelectech;7019
455;XeElectech;XE710;xeelectech;7019
456;XeElectech;XE710C;xeelectech;7019
457;XeElectech;XE800;xeelectech;7019
458;Vjoy;C20T;vjoycar;7020
459;Vjoy;C30T;vjoycar;7020
460;Vjoy;ET005;vjoycar;7020
461;Vjoy;ET006;vjoycar;7020
462;Vjoy;ET007;vjoycar;7020
463;Vjoy;ET102;vjoycar;7020
464;Vjoy;ET103;vjoycar;7020
465;Vjoy;ET201;vjoycar;7020
466;Vjoy;OT001;vjoycar;7020
467;Vjoy;OT002;vjoycar;7020
470;Vjoy;T0024;vjoycar;7020
471;Vjoy;T0026;vjoycar;7020
472;Vjoy;T10;vjoycar;7020
473;Vjoy;T1024;vjoycar;7020
474;Vjoy;T1080;vjoycar;7020
475;Vjoy;T1124;vjoycar;7020
476;Vjoy;T12;vjoycar;7020
477;Vjoy;T12SE;vjoycar;7020
478;Vjoy;T13;vjoycar;7020
479;Vjoy;T15400;vjoycar;7020
480;Vjoy;T16;vjoycar;7020
481;Vjoy;T18;vjoycar;7020
482;Vjoy;T18H;vjoycar;7020
483;Vjoy;T19;vjoycar;7020
484;Vjoy;T2024;vjoycar;7020
485;Vjoy;T2124;vjoycar;7020
486;Vjoy;T300;vjoycar;7020
487;Vjoy;T300S;vjoycar;7020
488;Vjoy;T3124;vjoycar;7020
489;Vjoy;T3180;vjoycar;7020
490;Vjoy;T4024;vjoycar;7020
491;Vjoy;T4400;vjoycar;7020
492;Vjoy;T500;vjoycar;7020
493;Vjoy;T500S;vjoycar;7020
494;Vjoy;T5010;vjoycar;7020
495;Vjoy;T5010S;vjoycar;7020
496;Vjoy;T5015;vjoycar;7020
497;Vjoy;T5015S;vjoycar;7020
498;Vjoy;T5020;vjoycar;7020
499;Vjoy;T5020S;vjoycar;7020
500;Vjoy;T510;vjoycar;7020
501;Vjoy;T5124;vjoycar;7020
502;Vjoy;T531W;vjoycar;7020
503;Vjoy;T532W;vjoycar;7020
504;Vjoy;T580;vjoycar;7020
505;Vjoy;T580W;vjoycar;7020
506;Vjoy;T6124;vjoycar;7020
507;Vjoy;T6124E;vjoycar;7020
508;Vjoy;T630;vjoycar;7020
509;Vjoy;T805;vjoycar;7020
510;Vjoy;T8124BMG;vjoycar;7020
511;Vjoy;T8800;vjoycar;7020
512;Vjoy;T903;vjoycar;7020
513;Vjoy;T905;vjoycar;7020
514;Vjoy;TK05;vjoycar;7020
515;Vjoy;TK05SE;vjoycar;7020
516;Vjoy;TK05SSE;vjoycar;7020
517;Vjoy;TK10;vjoycar;7020
518;Vjoy;TK101;vjoycar;7020
519;Vjoy;TK10SE;vjoycar;7020
520;Vjoy;TK10SSE;vjoycar;7020
521;Vjoy;TK15;vjoycar;7020
522;Vjoy;TK20;vjoycar;7020
525;Vjoy;TK20SE;vjoycar;7020
526;Vjoy;TK20SSE;vjoycar;7020
527;Vjoy;TK411S-4G;vjoycar;7020
528;Vjoy;TT0024;vjoycar;7020
529;Vjoy;TT330;vjoycar;7020
530;Eelink;GPT06;eelink;7021
531;Eelink;GPT06-W;eelink;7021
532;Eelink;GPT09;eelink;7021
533;Eelink;GPT12;eelink;7021
534;Eelink;GPT12-L;eelink;7021
535;Eelink;GPT19;eelink;7021
536;Eelink;GPT19-H;eelink;7021
537;Eelink;GPT29;eelink;7021
538;Eelink;GPT46;eelink;7021
539;Eelink;K20;eelink;7021
540;Eelink;K30;eelink;7021
541;Eelink;K9;eelink;7021
542;Eelink;OT08;eelink;7021
543;Eelink;OT10;eelink;7021
544;Eelink;P168;eelink;7021
545;Eelink;PT06-T;eelink;7021
546;Eelink;PT18;eelink;7021
547;Eelink;PT26;eelink;7021
548;Eelink;TK115;eelink;7021
549;Eelink;TK116;eelink;7021
550;Eelink;TK116-T;eelink;7021
551;Eelink;TK119-T;eelink;7021
552;Eelink;TK119-W;eelink;7021
553;Eelink;TK121;eelink;7021
554;Eelink;TK121-S;eelink;7021
555;Eelink;TK319-H;eelink;7021
556;Eelink;TK319L;eelink;7021
557;Eelink;TK419;eelink;7021
558;Gosafe;G1C;gosafe;7022
559;Gosafe;G1C Mini;gosafe;7022
560;Gosafe;G1RUS;gosafe;7022
561;Gosafe;G1S;gosafe;7022
562;Gosafe;G2P;gosafe;7022
563;Gosafe;G3A;gosafe;7022
564;Gosafe;G3S;gosafe;7022
565;Gosafe;G3SC;gosafe;7022
566;Gosafe;G6C;gosafe;7022
567;Gosafe;G6S;gosafe;7022
568;Gosafe;G71;gosafe;7022
569;Gosafe;G717;gosafe;7022
570;Gosafe;G737;gosafe;7022
571;Gosafe;G77;gosafe;7022
572;Gosafe;G777;gosafe;7022
573;Gosafe;G787;gosafe;7022
574;Gosafe;G79;gosafe;7022
575;Gosafe;G797;gosafe;7022
576;Gosafe;G797W;gosafe;7022
577;Gosafe;G7A7;gosafe;7022
578;Gosafe;G91i;gosafe;7022
579;Gosafe;GAT1000;gosafe;7022
580;Gosafe;GAT3000;gosafe;7022
581;Gosafe;GS16;gosafe;7022
582;Gosafe;GTU5000;gosafe;7022
583;Skypatrol;SP1603;skypatrol;7022
584;Skypatrol;SP2600;skypatrol;7022
585;Skypatrol;SP2603;skypatrol;7022
586;Skypatrol;SP3603;skypatrol;7022
587;Skypatrol;SP3801;skypatrol;7022
588;Skypatrol;SP3824;skypatrol;7022
589;Skypatrol;SP4603;skypatrol;7022
590;Skypatrol;SP5603;skypatrol;7022
591;Skypatrol;SP7600;skypatrol;7022
592;Skypatrol;SP8603;skypatrol;7022
593;Skypatrol;SP8703;skypatrol;7022
594;Skypatrol;SP8824;skypatrol;7022
595;Skypatrol;SP9603;skypatrol;7022
596;Xirgo;XT1040S6;xirgo;7024
597;Xirgo;XT2000G;xirgo;7024
598;Xirgo;XT2050C;xirgo;7024
599;Xirgo;XT2060G;xirgo;7024
600;Xirgo;XT2100;xirgo;7024
601;Xirgo;XT2150;xirgo;7024
602;Xirgo;XT2150C;xirgo;7024
603;Xirgo;XT2150G;xirgo;7024
604;Xirgo;XT2160G;xirgo;7024
605;Xirgo;XT2180;xirgo;7024
606;Xirgo;XT3200;xirgo;7024
607;Xirgo;XT4500;xirgo;7024
608;Xirgo;XT4500G;xirgo;7024
609;Xirgo;XT4550C;xirgo;7024
610;Xirgo;XT4560G;xirgo;7024
611;Xirgo;XT4700;xirgo;7024
612;Xirgo;XT4750C;xirgo;7024
613;Xirgo;XT4760;xirgo;7024
614;Xirgo;XT4760G5;xirgo;7024
615;Xirgo;XT4850C;xirgo;7024
616;Xirgo;XT4860G;xirgo;7024
617;Xirgo;XT4900;xirgo;7024
618;Xirgo;XT4975;xirgo;7024
619;Xirgo;XT5000;xirgo;7024
620;Xirgo;XT5050C;xirgo;7024
621;Xirgo;XT5060;xirgo;7024
622;Xirgo;XT6200;xirgo;7024
623;Xirgo;XT6260;xirgo;7024
624;Smartrack;KX300;smartrack;7025
625;Smartrack;KX301;smartrack;7025
626;Smartrack;KX302;smartrack;7025
627;Smartrack;KX402;smartrack;7025
628;ReachFar;RF-V03;reachfar;7026
629;ReachFar;RF-V10;reachfar;7026
630;ReachFar;RF-V11;reachfar;7026
631;ReachFar;RF-V16;reachfar;7026
632;ReachFar;RF-V18;reachfar;7026
633;ReachFar;RF-V20;reachfar;7026
634;ReachFar;RF-V26;reachfar;7026
635;ReachFar;RF-V26+;reachfar;7026
636;ReachFar;RF-V28;reachfar;7026
637;ReachFar;RF-V30;reachfar;7026
638;ReachFar;RF-V32;reachfar;7026
639;ReachFar;RF-V34;reachfar;7026
640;ReachFar;RF-V36;reachfar;7026
641;ReachFar;RF-V38;reachfar;7026
642;ReachFar;RF-V40;reachfar;7026
643;ReachFar;RF-V42;reachfar;7026
644;ReachFar;RF-V43;reachfar;7026
645;ReachFar;RF-V44;reachfar;7026
646;ReachFar;RF-V46;reachfar;7026
647;ReachFar;RF-V47;reachfar;7026
648;ReachFar;RF-V6;reachfar;7026
649;ReachFar;RF-V6+;reachfar;7026
650;ReachFar;RF-V7;reachfar;7026
651;ReachFar;RF-V8;reachfar;7026
652;ReachFar;RF-V8S;reachfar;7026
653;ReachFar;RF-V9;reachfar;7026
654;ICAR GPS;IK100;icargps;7027
655;ICAR GPS;IK101;icargps;7027
656;ICAR GPS;IK110;icargps;7027
657;ICAR GPS;IK111;icargps;7027
658;ICAR GPS;IK112;icargps;7027
659;ICAR GPS;IK120;icargps;7027
660;ICAR GPS;IK121;icargps;7027
661;ICAR GPS;IK122;icargps;7027
662;ICAR GPS;IK123;icargps;7027
663;ICAR GPS;IK140;icargps;7027
664;ICAR GPS;IK151;icargps;7027
665;ICAR GPS;IK200;icargps;7027
666;ICAR GPS;IK201;icargps;7027
667;ICAR GPS;IK202;icargps;7027
668;ICAR GPS;IK203;icargps;7027
669;ICAR GPS;IK204;icargps;7027
670;ICAR GPS;IK205;icargps;7027
671;ICAR GPS;IK206;icargps;7027
672;ICAR GPS;IK210;icargps;7027
673;ICAR GPS;IK220;icargps;7027
674;ICAR GPS;IK230;icargps;7027
675;ICAR GPS;IK240;icargps;7027
676;ICAR GPS;IK241;icargps;7027
677;ICAR GPS;IK242;icargps;7027
678;ICAR GPS;IK700;icargps;7027
679;ICAR GPS;IK702;icargps;7027
680;ICAR GPS;IK703;icargps;7027
681;ICAR GPS;IK710;icargps;7027
682;ICAR GPS;IK711;icargps;7027
683;ICAR GPS;IK720;icargps;7027
684;ICAR GPS;IK721;icargps;7027
685;ICAR GPS;IK722;icargps;7027
686;ICAR GPS;IK723;icargps;7027
687;ICAR GPS;IK726;icargps;7027
688;ICAR GPS;IK740;icargps;7027
689;ICAR GPS;IK741;icargps;7027
690;ICAR GPS;IK750;icargps;7027
691;i-Trac GPS;A1;itracgps;7028
692;i-Trac GPS;A1X;itracgps;7028
693;i-Trac GPS;A3;itracgps;7028
694;i-Trac GPS;AS2000+;itracgps;7028
695;i-Trac GPS;AS5000;itracgps;7028
696;i-Trac GPS;GTLT3;itracgps;7028
697;i-Trac GPS;MT-1;itracgps;7028
698;i-Trac GPS;MT1;itracgps;7028
699;i-Trac GPS;MT1C;itracgps;7028
700;i-Trac GPS;MT1X;itracgps;7028
701;i-Trac GPS;MT1Z;itracgps;7028
702;i-Trac GPS;VT1000;itracgps;7028
703;i-Trac GPS;VT600X;itracgps;7028
704;i-Trac GPS;Y3;itracgps;7028
705;Alematics;AE1;alematics;7029
706;Alematics;AE1-W;alematics;7029
707;Alematics;AE3;alematics;7029
708;Alematics;AM1;alematics;7029
709;Alematics;AM1-S;alematics;7029
710;Alematics;AM1-W;alematics;7029
711;Alematics;AM3;alematics;7029
712;Alematics;AM3-S;alematics;7029
713;Alematics;AM3-W;alematics;7029
714;Alematics;AM7;alematics;7029
715;Alematics;AM7-S;alematics;7029
716;Alematics;AM7-W;alematics;7029
717;Alematics;AP1;alematics;7029
718;Alematics;AP3;alematics;7029
719;Alematics;AP7;alematics;7029
720;Alematics;AT1;alematics;7029
721;Alematics;AT1-C;alematics;7029
722;Alematics;AT3;alematics;7029
723;Alematics;AT3-C;alematics;7029
724;Alematics;AT7;alematics;7029
725;Alematics;AT7-C;alematics;7029
726;Pretrace;TC55;pretrace;7030
727;Pretrace;TC56;pretrace;7030
728;Pretrace;TC56L;pretrace;7030
729;Pretrace;TC56W;pretrace;7030
730;Pretrace;TC80;pretrace;7030
731;Pretrace;TC85;pretrace;7030
732;Pretrace;TC85D;pretrace;7030
733;Arknav;AT-04;arknav;7031
734;Arknav;AT-433;arknav;7031
735;Arknav;AT-5000;arknav;7031
736;Arknav;AT-9000;arknav;7031
737;Arknav;CT-X8;arknav;7031
738;Arknav;DX-3;arknav;7031
739;Arknav;IR-7;arknav;7031
740;Arknav;R-9PRO;arknav;7031
741;Arknav;R-9W;arknav;7031
742;Arknav;RV-8;arknav;7031
743;Arknav;RV-9W;arknav;7031
744;Arknav;RX-10;arknav;7031
745;Arknav;RX-8W;arknav;7031
746;Arknav;RX-9;arknav;7031
747;Arknav;TDVR 42D;arknav;7031
748;Haicom;HI-602X;haicom;7032
749;Haicom;HI-603X;haicom;7032
750;Haicom;HI-604X;haicom;7032
751;Haicom;HI-605X;haicom;7032
752;CarTrackGPS;iTrackPRO ADVANCE;cartrackgps;7033
753;CarTrackGPS;iTrackPRO AVL;cartrackgps;7033
754;CarTrackGPS;iTrackPRO BRACELET;cartrackgps;7033
755;CarTrackGPS;iTrackPRO CANBUS;cartrackgps;7033
756;CarTrackGPS;iTrackPRO CAR;cartrackgps;7033
757;CarTrackGPS;iTrackPRO HUNTER;cartrackgps;7033
758;CarTrackGPS;iTrackPRO PORTABLE;cartrackgps;7033
759;CarTrackGPS;iTrackPRO VLOGIC;cartrackgps;7033
760;KingSword;ET-01;kingsword;7034
761;KingSword;ET-02;kingsword;7034
762;Auto Leaders;AL-900C;amwell;7035
763;Auto Leaders;AL-900E;amwell;7035
764;Auto Leaders;AL-900G;amwell;7035
765;KHD;KC200;amwell;7035
766;KHD;KG100;amwell;7035
767;KHD;KG200;amwell;7035
768;KHD;KG300;amwell;7035
769;STG;T100;amwell;7035
770;STG;T360;amwell;7035
771;Amwell;T360-101A;amwell;7035
772;Amwell;T360-101E;amwell;7035
773;Amwell;T360-101P;amwell;7035
774;Amwell;T360-103;amwell;7035
775;Amwell;T360-106;amwell;7035
776;Amwell;T360-108;amwell;7035
777;Amwell;T360-269;amwell;7035
778;Amwell;T360-269B;amwell;7035
779;Amwell;T360-269JT;amwell;7035
780;STG;T50;amwell;7035
781;San Jose Technology;CT-24;sanav;7036
782;San Jose Technology;CT-58;sanav;7036
783;San Jose Technology;GC-101;sanav;7036
784;San Jose Technology;GS-818;sanav;7036
785;San Jose Technology;GU-819;sanav;7036
786;San Jose Technology;GX-101;sanav;7036
787;San Jose Technology;MT-101;sanav;7036
788;San Jose Technology;MT-102;sanav;7036
789;San Jose Technology;MU-201;sanav;7036
790;San Jose Technology;MU-201S2;sanav;7036
791;San Jose Technology;MU-201S3;sanav;7036
792;San Jose Technology;QG-201;sanav;7036
793;GOTOP;G01;gotop;7037
794;GOTOP;G01-3G;gotop;7037
795;GOTOP;G01-4G;gotop;7037
796;GOTOP;G02;gotop;7037
797;GOTOP;G07;gotop;7037
798;GOTOP;G08;gotop;7037
799;GOTOP;G09;gotop;7037
800;GOTOP;G10;gotop;7037
801;GOTOP;G100;gotop;7037
802;GOTOP;G20;gotop;7037
803;GOTOP;G23;gotop;7037
804;GOTOP;G23D;gotop;7037
805;GOTOP;G23N;gotop;7037
806;GOTOP;G30;gotop;7037
807;GOTOP;G50;gotop;7037
808;GOTOP;GX5-4G;gotop;7037
809;GOTOP;GX6-4G;gotop;7037
810;GOTOP;T0500;gotop;7037
811;GOTOP;T1010;gotop;7037
812;GOTOP;T1200;gotop;7037
813;GOTOP;T1600;gotop;7037
814;GOTOP;T1900;gotop;7037
815;GOTOP;T5000;gotop;7037
816;GOTOP;T500S;gotop;7037
817;GOTOP;T5800;gotop;7037
818;GOTOP;T6300;gotop;7037
819;GOTOP;TE-200;gotop;7037
820;GOTOP;TE-207S;gotop;7037
821;GOTOP;TK-120;gotop;7037
822;GOTOP;TK-206;gotop;7037
823;GOTOP;TK-208;gotop;7037
824;GOTOP;TK-209B;gotop;7037
825;GOTOP;TK-720;gotop;7037
826;GOTOP;VT-321;gotop;7037
827;GOTOP;VT-360;gotop;7037
828;GOTOP;VT-360A;gotop;7037
829;GOTOP;VT-380A;gotop;7037
830;GOTOP;VT-390;gotop;7037
831;GOTOP;VT-391;gotop;7037
832;GOTOP;VT-392;gotop;7037
833;GOTOP;VT393;gotop;7037
834;GOTOP;X300C;gotop;7037
835;GOTOP;X300N;gotop;7037
836;GOTOP;X6180;gotop;7037
837;GlobalSat;GTR-388C1;globalsat;7038
838;GlobalSat;LT-100;globalsat;7038
839;GlobalSat;LT-20;globalsat;7038
840;GlobalSat;LT-200E;globalsat;7038
841;GlobalSat;LT-200H;globalsat;7038
842;GlobalSat;LT-20R;globalsat;7038
843;GlobalSat;LW-360HR;globalsat;7038
844;GlobalSat;NW-360HR;globalsat;7038
845;GlobalSat;TR-350;globalsat;7038
846;GlobalSat;TR-616C1;globalsat;7038
847;GlobalSat;TR-900;globalsat;7038
848;Laipac Technology;S911 Bracelet Locator;laipac;7052
849;Laipac Technology;S911 Bracelet Locator HC;laipac;7052
850;Laipac Technology;S911 Bracelet Locator ST;laipac;7052
851;Laipac Technology;S911 Enforcer;laipac;7052
852;Laipac Technology;S911 Lola;laipac;7052
853;Laipac Technology;S911 Personal Locator;laipac;7052
854;Laipac Technology;Starfinder Aire;laipac;7052
855;Laipac Technology;Starfinder Bus;laipac;7052
856;Laipac Technology;Starfinder Kamel;laipac;7052
857;Laipac Technology;Starfinder Lite;laipac;7052
858;GoPass;AVL-900;gopass;7039
859;GoPass;AVL-900-2;gopass;7039
860;GoPass;AVL-900-3;gopass;7039
861;GoPass;AVL-900(M);gopass;7039
862;GoPass;AVL-900(R);gopass;7039
863;GoPass;AVL-900e;gopass;7039
864;GoPass;AVL-901(B);gopass;7039
865;GoPass;AVL-901(C);gopass;7039
866;GoPass;AVL-901(D);gopass;7039
867;GoPass;AVL-921;gopass;7039
868;GoPass;AVL-922;gopass;7039
869;GoPass;AVL-926;gopass;7039
870;GoPass;GPS-911(M);gopass;7039
871;GoPass;GPS-932;gopass;7039
872;GoPass;GPS-990;gopass;7039
873;GoPass;GPX06s;gopass;7039
874;GoPass;GPX10L;gopass;7039
875;GoPass;GPX10XL;gopass;7039
876;GoPass;GPX803;gopass;7039
885;Jointech;GP4000;jointech;7040
886;Jointech;GP4000A;jointech;7040
887;Jointech;GP5000;jointech;7040
888;Jointech;GP6000;jointech;7040
889;Jointech;GP6000F;jointech;7040
890;Jointech;JT600;jointech;7040
891;Jointech;JT600B;jointech;7040
892;Jointech;JT600C;jointech;7040
893;Jointech;JT700;jointech;7040
894;Jointech;JT700DT;jointech;7040
895;Jointech;JT701;jointech;7040
896;Jointech;JT703;jointech;7040
897;Jointech;JT704;jointech;7040
898;Jointech;JT705;jointech;7040
899;Jointech;JT707;jointech;7040
900;Jointech;JT707A;jointech;7040
901;Meitrack;T622G;meitrack;7001
902;Meitrack;T622G_F9;meitrack;7001
903;Meitrack;T633L;meitrack;7001
904;Meitrack;TC68L;meitrack;7001
905;Meitrack;TC68SG;meitrack;7001
906;Meitrack;TC68SL;meitrack;7001
907;Meitrack;TS299L;meitrack;7001
908;Meitrack;K211G;meitrack;7001
909;Meitrack;P88L;meitrack;7001
910;Meitrack;P99E;meitrack;7001
911;Meitrack;P99G;meitrack;7001
912;Meitrack;P99L;meitrack;7001
913;Meitrack;T355G;meitrack;7001
914;Meitrack;T622;meitrack;7001
915;Meitrack;T622E;meitrack;7001
916;ERM;StarLink Asset;erm;7051
917;ERM;StarLink eConnect;erm;7051
918;ERM;StarLink EV;erm;7051
919;ERM;StarLink OBD;erm;7051
920;ERM;StarLink One;erm;7051
921;ERM;StarLink SVR;erm;7051
922;ERM;StarLink Tracker;erm;7051
923;ERM;StarLink Tracker SF;erm;7051
924;ERM;StarLink Tracker SR;erm;7051
925;ERM;StarLink TrackerCAN;erm;7051
926;ERM;StarLink TrackerWi-FI;erm;7051
927;ERM;StarLink Voice;erm;7051
928;TOPFLYtech;T8608;topfly;7050
929;TOPFLYtech;T8608D;topfly;7050
930;TOPFLYtech;T8801;topfly;7050
931;TOPFLYtech;T8802;topfly;7050
933;TOPFLYtech;T8803+E;topfly;7050
934;TOPFLYtech;T8806+R;topfly;7050
935;TOPFLYtech;T8808+;topfly;7050
936;TOPFLYtech;T8901;topfly;7050
937;TOPFLYtech;TLD1;topfly;7050
938;TOPFLYtech;TLD1-D;topfly;7050
939;TOPFLYtech;TLP1-LF;topfly;7050
940;TOPFLYtech;TLP1-P;topfly;7050
941;TOPFLYtech;TLP1-SF;topfly;7050
942;TOPFLYtech;TLP2-SF;topfly;7050
943;TOPFLYtech;TLP2-SFB;topfly;7050
944;TOPFLYtech;TLW1-10;topfly;7050
945;TOPFLYtech;TLW1-8;topfly;7050
946;TOPFLYtech;TLW2-9G;topfly;7050
947;TOPFLYtech;TLW2-9GB;topfly;7050
948;Bofan;PT03;bofan;7042
949;Bofan;PT100;bofan;7042
950;Bofan;PT200;bofan;7042
951;Bofan;PT201;bofan;7042
952;Bofan;PT30;bofan;7042
953;Bofan;PT300X;bofan;7042
954;Bofan;PT500;bofan;7042
955;Bofan;PT502;bofan;7042
956;Bofan;PT510;bofan;7042
957;Bofan;PT520;bofan;7042
958;Bofan;PT521;bofan;7042
959;Bofan;PT60;bofan;7042
960;Bofan;PT600X;bofan;7042
961;Bofan;PT80;bofan;7042
962;Diwei;DT20;eelink;7021
963;Diwei;DVT100;eelink;7021
964;Diwei;GOT08;eelink;7021
965;Diwei;GOT10;eelink;7021
966;Diwei;GPT06-W;eelink;7021
967;Diwei;GPT106;eelink;7021
968;Diwei;GPT12;eelink;7021
969;Diwei;GPT12-L;eelink;7021
970;Diwei;GPT15;eelink;7021
971;Diwei;GPT19-H;eelink;7021
972;Diwei;GPT19-H;eelink;7021
973;Diwei;GPT29;eelink;7021
974;Diwei;GPT46;eelink;7021
975;Diwei;GT03A;eelink;7021
976;Diwei;GT06;eelink;7021
977;Diwei;GT60;eelink;7021
978;Diwei;K30;eelink;7021
979;Diwei;K9;eelink;7021
980;Diwei;MGT90;eelink;7021
981;Diwei;O168;eelink;7021
982;Diwei;OT08;eelink;7021
983;Diwei;PT06-T;eelink;7021
984;Diwei;PT26;eelink;7021
985;Diwei;TK101;eelink;7021
986;Diwei;TK101;eelink;7021
987;Diwei;TK103;eelink;7021
988;Diwei;TK105;eelink;7021
989;Diwei;TK108;eelink;7021
990;Diwei;TK112;eelink;7021
991;Diwei;TK115;eelink;7021
992;Diwei;TK116;eelink;7021
993;Diwei;TK118;eelink;7021
994;Diwei;TK119-T;eelink;7021
995;Diwei;TK119-W;eelink;7021
996;Diwei;TK121;eelink;7021
997;Diwei;TK121-S;eelink;7021
998;Diwei;TK201;eelink;7021
999;Diwei;TK202;eelink;7021
1000;Diwei;TK319;eelink;7021
1001;Diwei;TK319-H;eelink;7021
1002;Diwei;TK319-H;eelink;7021
1003;Diwei;TK319-L;eelink;7021
1004;Diwei;TK419;eelink;7021
1005;Diwei;VT310;eelink;7021
1006;Heacent;908;keson;7041
1007;Heacent;HC06A;keson;7041
1008;KeSon;KS-3G;keson;7041
1009;KeSon;KS106;keson;7041
1010;KeSon;KS108;keson;7041
1011;KeSon;KS158;keson;7041
1012;KeSon;KS158;keson;7041
1013;KeSon;KS166;keson;7041
1014;KeSon;KS168;keson;7041
1015;KeSon;KS168F;keson;7041
1016;KeSon;KS168M;keson;7041
1017;KeSon;KS168M;keson;7041
1018;KeSon;KS168T;keson;7041
1019;KeSon;KS168V;keson;7041
1020;KeSon;KS189;keson;7041
1021;KeSon;KS1998;keson;7041
1022;KeSon;KS199G;keson;7041
1023;KeSon;KS2016;keson;7041
1024;KeSon;KS365;keson;7041
1025;KeSon;KS668;keson;7041
1026;KeSon;KS668;keson;7041
1027;KeSon;KS668;keson;7041
1028;KeSon;KS668-OBD;keson;7041
1029;KeSon;KS668H;keson;7041
1030;KeSon;S188;keson;7041
1031;N/A;HC207;keson;7041
1032;N/A;P10;keson;7041
1033;N/A;PT200;keson;7041
1034;N/A;PT350;keson;7041
1035;N/A;TK118;keson;7041
1036;N/A;TL201;keson;7041
1037;N/A;V680;keson;7041
1038;N/A;VT810;keson;7041
1039;Autoseeker;AT-1;vsun;7043
1040;Autoseeker;AT-12;vsun;7043
1041;Autoseeker;AT-12A;vsun;7043
1042;Autoseeker;AT-17;vsun;7043
1043;Autoseeker;AT-17B;vsun;7043
1044;Autoseeker;AT-19;vsun;7043
1045;Mictrack;MC300;vsun;7043
1046;Mictrack;MP10;vsun;7043
1047;Mictrack;MP80;vsun;7043
1048;Mictrack;MP90;vsun;7043
1049;Mictrack;MP90-NB;vsun;7043
1050;Mictrack;MT500;vsun;7043
1051;Mictrack;MT510;vsun;7043
1052;Mictrack;MT510-G;vsun;7043
1053;Mictrack;MT530;vsun;7043
1054;Mictrack;MT550;vsun;7043
1055;Mictrack;MT600;vsun;7043
1056;Mictrack;MT810;vsun;7043
1057;Mictrack;MT821;vsun;7043
1058;Mictrack;MT825;vsun;7043
1059;V-Sun;TLT-1B;vsun;7043
1060;V-Sun;TLT-1C;vsun;7043
1061;V-Sun;TLT-1D;vsun;7043
1062;V-Sun;TLT-1F;vsun;7043
1063;V-Sun;TLT-2F;vsun;7043
1064;V-Sun;TLT-2H;vsun;7043
1065;V-Sun;TLT-2K;vsun;7043
1066;V-Sun;TLT-2N;vsun;7043
1067;V-Sun;TLT-3;vsun;7043
1068;V-Sun;TLT-3A;vsun;7043
1069;V-Sun;TLT-6C;vsun;7043
1070;V-Sun;TLT-7;vsun;7043
1071;V-Sun;TLT-7B;vsun;7043
1072;V-Sun;TLT-8A;vsun;7043
1073;V-Sun;TLT-8B;vsun;7043
1074;V-Sun;V3880;vsun;7043
1075;V-Sun;V520;vsun;7043
1076;V-Sun;V520-1;vsun;7043
1077;V-Sun;V520-A;vsun;7043
1078;V-Sun;V526;vsun;7043
1079;V-Sun;V526-A;vsun;7043
1080;V-Sun;V530;vsun;7043
1081;V-Sun;V530-B;vsun;7043
1082;V-Sun;V580;vsun;7043
1083;V-Sun;V680-A;vsun;7043
1084;V-Sun;V690;vsun;7043
1085;Blue Idea;T1;blueidea;7044
1086;Blue Idea;V01;blueidea;7044
1087;Blue Idea;V05;blueidea;7044
1088;Blue Idea;V03;blueidea;7044
1089;Blue Idea;T3;blueidea;7044
1090;Blue Idea;V07;blueidea;7044
1091;Blue Idea;MC500;blueidea;7044
1092;Blue Idea;ET700;blueidea;7044
1093;Blue Idea;ET800E;blueidea;7044
1094;Blue Idea;ET800A;blueidea;7044
1095;Blue Idea;MC600;blueidea;7044
1096;Man Power Technology;MP2030A;manpower;7045
1097;Man Power Technology;MP2030B;manpower;7045
1098;Man Power Technology;MP2031A;manpower;7045
1099;Man Power Technology;MP2031B;manpower;7045
1100;Man Power Technology;MP2031C;manpower;7045
1101;Wonde Proud;VT-10;wondeproud;7046
1102;Wonde Proud;VT-300;wondeproud;7046
1103;Wonde Proud;VT-360;wondeproud;7046
1104;Wonde Proud;M7;wondeproud;7046
1105;Wonde Proud;M7 D2;wondeproud;7046
1106;Wonde Proud;OT-10;wondeproud;7046
1107;Wonde Proud;TT-1;wondeproud;7046
1108;Wonde Proud;SPT-10;wondeproud;7046
1109;N/A;TK5000;wondeproud;7046
1110;N/A;TK5000XL;wondeproud;7046
1111;GPS Marker;E130;gpsmarker;7047
1112;GPS Marker;M100;gpsmarker;7047
1113;GPS Marker;M130;gpsmarker;7047
1114;GPS Marker;M60;gpsmarker;7047
1115;GPS Marker;M70;gpsmarker;7047
1116;GPS Marker;M80;gpsmarker;7047
1117;Defenstar;DS007;eview;7048
1118;Defenstar;DS811;eview;7048
1119;Defenstar;GPS668;eview;7048
1120;Eview;EV-04;eview;7048
1121;Eview;EV-05;eview;7048
1122;Eview;EV-07;eview;7048
1123;Eview;EV-07B;eview;7048
1124;Eview;EV-07P;eview;7048
1125;Eview;EV-08;eview;7048
1126;Eview;EV-09;eview;7048
1127;Eview;EV-201;eview;7048
1128;Eview;EV-202;eview;7048
1129;Eview;EV-601;eview;7048
1130;Eview;EV-602;eview;7048
1131;Eview;EV-603;eview;7048
1132;Eview;EV-606;eview;7048
1133;minifinder;Atto;eview;7048
1134;minifinder;Pico;eview;7048
1135;SaR-mini;FAM;eview;7048
1136;SaR-mini;PCB;eview;7048
1137;SaR-mini;WSG;eview;7048
1138;Freedom;PT-9;freedom;7049
1139;Freedom;PT-10;freedom;7049
1140;Defenstar;DS008;eview;7048
1141;TOPFLYtech;TLW1-4;topfly;7050
1142;Navtelecom;SIGNAL;navtelecom;7054
1143;Navtelecom;SIGNAL S-2651;navtelecom;7054
1144;Navtelecom;SIGNAL S-2653;navtelecom;7054
1145;Navtelecom;SMART S-2420 EASY;navtelecom;7054
1146;Navtelecom;SMART S-2420 EASY+;navtelecom;7054
1147;Navtelecom;SMART S-2422 MID;navtelecom;7054
1148;Navtelecom;SMART S-2423 MID+;navtelecom;7054
1149;Navtelecom;SMART S-2425 COMPLEX;navtelecom;7054
1150;Navtelecom;SMART S-242X;navtelecom;7054
1151;Navtelecom;SMART S-2433 HIT;navtelecom;7054
1152;Navtelecom;SMART S-2435 MAX;navtelecom;7054
1153;Navtelecom;SMART S-243X;navtelecom;7054
1154;Galileosky;7x;galileosky;7055
1155;Galileosky;7x 3G;galileosky;7055
1156;Galileosky;7x Plus;galileosky;7055
1157;Galileosky;7.0;galileosky;7055
1158;Galileosky;7x C;galileosky;7055
1159;Galileosky;7.0 Wi-Fi;galileosky;7055
1160;Galileosky;Base Block Lite;galileosky;7055
1161;Galileosky;7.0 Lite;galileosky;7055
1162;Galileosky;Base Block Optimum;galileosky;7055
1163;Galileosky;OBD-II;galileosky;7055
1164;Galileosky;Base Block Wi-Fi;galileosky;7055
1165;Galileosky;Base Block Wi-Fi Hub;galileosky;7055
1166;Galileosky;Base Block Iridium;galileosky;7055
1167;Ruptela;FM-Eco3;ruptela;7056
1168;Ruptela;FM-Eco4 light S;ruptela;7056
1169;Ruptela;FM-Eco4 light+ 3G T;ruptela;7056
1170;Ruptela;FM-Eco4+ 3G T;ruptela;7056
1171;Ruptela;FM-Eco4+ S;ruptela;7056
1172;Ruptela;FM-Plug4;ruptela;7056
1173;Ruptela;FM-Pro3;ruptela;7056
1174;Ruptela;FM-Pro4;ruptela;7056
1175;Ruptela;FM-Tco3;ruptela;7056
1176;Ruptela;FM-Tco4 HCV;ruptela;7056
1177;Ruptela;FM-Tco4 LCV;ruptela;7056
1178;Arusnavi;ARNAVI 5;arusnavi;7057
1179;Arusnavi;ARNAVI INTEGRAL 3;arusnavi;7057
1180;Arusnavi;ARNAVI BEACON;arusnavi;7057
1181;Arusnavi;ARNAVI ESM-L;arusnavi;7057
1182;Arusnavi;ARNAVI ESM-P;arusnavi;7057
1183;Neomatica;ADM700 3G;neomatica;7058
1184;Neomatica;ADM007 BLE;neomatica;7058
1185;Neomatica;ADM100;neomatica;7058
1186;Neomatica;ADM700;neomatica;7058
1187;Neomatica;ADM300;neomatica;7058
1188;Neomatica;ADM333;neomatica;7058
1189;Neomatica;ADM333 BLE;neomatica;7058
1190;Neomatica;ADM50;neomatica;7058
1191;BITREK;BI 530P TREK;teltonika;7002
1192;BITREK;BI 530 TREK;teltonika;7002
1193;BITREK;BI 520L TREK;teltonika;7002
1194;BITREK;BI 520 TREK;teltonika;7002
1195;BITREK;BI 520R TREK;teltonika;7002
1196;BITREK;BI 820 TREK OBD;teltonika;7002
1197;BITREK;BI 920 TREK;teltonika;7002
1198;BITREK;BI 310 CICADA;teltonika;7002
1199;BITREK;BI 810 TREK;teltonika;7002
1200;BITREK;BI 910 TREK;teltonika;7002
1201;BITREK;BI 820 TREK;teltonika;7002
1202;BITREK;BI 810;teltonika;7002
1203;BITREK;BI 810 CONNECT;teltonika;7002
1204;Satellite Solutions;SAT-LITE 3;satellite;7059
1205;Satellite Solutions;SAT-LITE 4;satellite;7059
1206;Autofon;Alpha-2XL-Mayak;autofon;7060
1207;Autofon;Omega;autofon;7060
1208;Autofon;Mayak D;autofon;7060
1209;Autofon;Alpha-XL-Mayak;autofon;7060
1210;Autofon;Micro Mayak;autofon;7060
1211;Autofon;Omega v2;autofon;7060
1212;Autofon;Mayak D-moto;autofon;7060
1213;Autofon;Mayak v8;autofon;7060
1214;Autofon;Alpha-Mayak;autofon;7060
1215;Autofon;Mayak;autofon;7060
1216;Autofon;Mayak SE;autofon;7060
1217;Autofon;Mayak SE+;autofon;7060
1218;Autofon;Mayak v5;autofon;7060
1219;Autofon;Mayak v7;autofon;7060
1220;StarLine;M10;autofon;7060
1221;StarLine;M11;autofon;7060
1222;StarLine;M16;autofon;7060
1223;StarLine;M17;autofon;7060
1224;StarLine;M96 SL;autofon;7060
1225;StarLine;M96 M;autofon;7060
1226;StarLine;A96;autofon;7060
1227;StarLine;T94;autofon;7060
1228;StarLine;M36;autofon;7060
1229;StarLine;A61;autofon;7060
1230;ATrack;AK1;atrack;7061
1231;ATrack;AK11;atrack;7061
1232;ATrack;AK7(S);atrack;7061
1233;ATrack;AK7V;atrack;7061
1234;ATrack;AL1;atrack;7061
1235;ATrack;AL11;atrack;7061
1236;ATrack;AL7;atrack;7061
1237;ATrack;AP1;atrack;7061
1238;ATrack;AP3;atrack;7061
1239;ATrack;AS1(E);atrack;7061
1240;ATrack;AS11;atrack;7061
1241;ATrack;AS3P(B);atrack;7061
1242;ATrack;AT1;atrack;7061
1243;ATrack;AT1 Pro;atrack;7061
1244;ATrack;AT3;atrack;7061
1245;ATrack;AT5;atrack;7061
1246;ATrack;AT5i;atrack;7061
1247;ATrack;AU5;atrack;7061
1248;ATrack;AU5i;atrack;7061
1249;ATrack;AU7;atrack;7061
1250;ATrack;AX11;atrack;7061
1251;ATrack;AX5;atrack;7061
1252;ATrack;AX5C;atrack;7061
1253;ATrack;AX7;atrack;7061
1254;ATrack;AX7B;atrack;7061
1255;ATrack;AX7P;atrack;7061
1256;ATrack;AX9;atrack;7061
1257;ATrack;AY5;atrack;7061
1258;ATrack;AY5i;atrack;7061
";
}