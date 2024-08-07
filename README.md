# GProject_Script / IRREGULAR
- It's the script of graduation work.<br/> 
* 졸업 작품으로 만든 게임의 코드를 업로드 했습니다.<br/> 

## Genre<br/> 
- 3D LOGLIKE

## Take On Part
- Level Design And Level Production<br/>
- Create Player<br/>
- Item Production<br/>
- Gimmick Production<br/>
- interaction<br/>
- Error Correction<br/>

## Explanation<br/>
- It's a game where you clear stages from multiple maps, get equipment, and use it to get rid of monsters or bosses.<br/>
* 여러개의 맵에서 스테이지를 클리어하며 장비를 얻고 이를 통해 몬스터 혹은 보스를 처치해 나아가는 게임입니다.<br/>
- It currently consists of a total of three scenes, and each scene contains one gimmick.<br/>
* 현재 총 3개의 씬으로 구성되어 있으며 각 씬마다 한개씩의 기믹이 포함되어 있습니다.<br/>
- The theme of each scene is desert, forest, and snow mountain area.<br/>
* 각 씬의 테마로는 사막, 숲, 눈산지역으로 이루어져 있습니다.

## Exemplary Photography

![주석 2023-12-27 180106](https://github.com/sju1026/GProject_Script/assets/128655662/e844fdfc-6755-4400-9434-3d34af36a428)<br/>

<h3>The First Picture</h3>
UI<br/>
- 좌측 상단 : Play Time<br/>
- 우측 상단 : MiniMap<br/>
- 좌측 하단 : Player State<br/>
- 우측 하단 : CoolTime<br/>
Explanation<br/>
- 처음 시작맵인 사막 맵에서 몬스터와 대치하는 상황입니다<br/>
- 몬스터는 죽으면 Star(스킬 강화 아이템)와 Heart(체력 회복 아이템)을 랜덤적인 확률로 드랍합니다<br/>
- 각 Stage마다 몬스터가 존재하며 몬스터를 전부 처치하지 않을 경우에는 Gate가 막아 이동이 불가능합니다<br/><br/>

![주석 2023-12-27 182928](https://github.com/sju1026/GProject_Script/assets/128655662/047257cf-52c0-4ab8-a416-03ffe394897d)<br/>

<h3>The Second Picture</h3>
Explanation<br/>
- 기믹의 예시 사진입니다<br/>
- 이 기믹은 5개의 기둥으로 존재하며 서로 다른 기둥들과 연결되어 한 기둥을 공격하였을 때 Light가 On/Off됩니다.<br/>
- 전체 기둥을 On시킨 경우 보이는 Box와 TP에 사용되는 Cube가 생성됩니다<br/>
- Box의 경우 BossStage에 입장하게 해주는 Key를 무조건 Drop하게 되며 이는 보스 처치 후 다음 씬으로 이동하면 초기화 됩니다<br/><br/>

![주석 2023-12-27 183133](https://github.com/sju1026/GProject_Script/assets/128655662/59f5fa7f-2311-4ca7-912f-194901ed734b)<br/>

<h3>The Third Picture</h3>
Explanation<br/>
- 전투에 예시 사진입니다<br/>
- 상자가 있는 방이 존재하며 몬스터를 처치하고 상자를 상호작용하면 Player State강화 아이템이 생성됩니다<br/>

## GantChart
![IRREGULAR_Gandchartt](https://github.com/sju1026/GProject_Script/assets/128655662/e35638bc-7d1e-4444-be1d-8a8953749314)
