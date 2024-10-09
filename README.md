# **IRREGULAR**
##### - Graduation work -

<hr>

<!--목차-->
## **목차**
- [**Project**](#project)
    - [Genre](#genre)
    - [Explanation](#explanation)
    - [Take On Part](#take-on-part)
    - [Techniques](#techniques)
- [**Scene**](#scene)
    - [Player Viewport](#player-viewport)
    - [World Gimic](#world-gimic)
    - [Interaction](#interaction)
- [**Gant Chart**](#gant-chart)
- [**Package**](#package)
- [**Contact**](#contact)

<hr>

<!--프로젝트 설명-->
## 📁**Project**
- 해당 프로젝트는 대학 **Graduation work**로 진행한 프로젝트입니다.
- 총 3인 개발로 기획자 1명, 개발자 2명으로 구성되었습니다.
- 게임 장르에 대한 아이디어는 **하이브**게임에서 가져왔습니다.

### 💡**Genre**
- **3D LOGLIKE**
-> Player가 사망하는 경우 얻은 능력치는 소멸되며 처음부터 다시 시작됩니다.

### 📖**Explanation**
- 여러 맵에서 스테이지를 클리어하며 장비 및 아이템을 통해 능력치를 증가시켜 몬스터 혹은 보스를 처치해 나아가는 게임입니다.
- 현재 총 3개의 씬으로 구성되어 있으며 각 씬마다 한 개씩의 기믹이 포함되어 있습니다.
- 각 씬의 테마로는 사막, 숲, 눈지역으로 이루어져 있습니다.

### 🎫**Take On Part**
- Level Design And Level Production
- Create Player
- Item Production
- Gimmick Production
- interaction
- Error Correction

### 💻**Techniques**
- [**Unity**]
-> Ver 2021.3.23f1
- [**C#**]
- [**Visual Studio**]

<hr>

## 🎬**Scene**
### **Player Viewport**
![주석 2023-12-27 180106](https://github.com/sju1026/GProject_Script/assets/128655662/e844fdfc-6755-4400-9434-3d34af36a428)
###### **UI**
- 좌측 상단 : Play Time
- 우측 상단 : MiniMap
- 좌측 하단 : Player State
- 우측 하단 : CoolTime

###### **Explanation**
- 처음 시작맵인 사막 맵에서 몬스터와 대치하는 상황입니다.
- 몬스터는 죽으면 Star(스킬 강화 아이템)와 Heart(체력 회복 아이템)을 랜덤적인 확률로 드랍합니다.
- 각 Stage마다 몬스터가 존재하며 몬스터를 전부 처치하지 않을 경우에는 Gate가 막아 이동이 불가능합니다.

<br>

### **World Gimic**
![주석 2023-12-27 182928](https://github.com/sju1026/GProject_Script/assets/128655662/047257cf-52c0-4ab8-a416-03ffe394897d)
###### **Explanation**
- 기믹의 예시 사진입니다.
- 이 기믹은 5개의 기둥으로 존재하며 서로 다른 기둥들과 연결되어 한 기둥을 공격하였을 때 Light가 On/Off됩니다.
- 전체 기둥을 On시킨 경우 보이는 Box와 TP에 사용되는 Cube가 생성됩니다.
- Box의 경우 BossStage에 입장하게 해주는 Key를 무조건 Drop하게 되며 이는 보스 처치 후 다음 씬으로 이동하면 초기화 됩니다.

<br>

### **Interaction**
![주석 2023-12-27 183133](https://github.com/sju1026/GProject_Script/assets/128655662/59f5fa7f-2311-4ca7-912f-194901ed734b)
###### **Explanation**
- 전투에 예시 사진입니다.
- 상자가 있는 방이 존재하며 몬스터를 처치하고 상자를 상호작용하면 Player State강화 아이템이 생성됩니다.

<hr>

<!-- 간트차트 -->
## 📈**Gant Chart**
![IRREGULAR_Gandchartt](https://github.com/sju1026/GProject_Script/assets/128655662/e35638bc-7d1e-4444-be1d-8a8953749314)

<!-- 사용한 패키지 -->
## 📒**Package**
- 모델링의 경우 대부분 Asset Store의 제품을 이용하였습니다.
- PolygonFantasyHeroCharacters 외 15개 (모델링 위주)

<!--접근-->
## 📫**Contact**
- 📧  **wodnd565@gmail.com**
- 📞  **010 - 5657 - 4813**

<hr>
