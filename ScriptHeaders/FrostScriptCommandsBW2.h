void c0x0();
void c0x1();
void End();
void ReturnAfterDelay(short p0);
void CallRoutine(int p0);
void Return();
void Logic06(short p0);
void Logic07(short p0);
void StackPushConst(short p0);
void StackPushVar(short p0);
void StackPop(short returnVar);
void StackDiscard();
void StackAdd();
void StackSub();
void StackMult();
void StackDiv();
void StoreFlag(short p0);
// == - 1
// != - 5
// > - 2
// >= - 4
// < - 0
// <= - 3
void StackCompare(short p0);
void BitwiseAndVar(short var, short value);
void BitwiseOrVar(short p0, short p1);
void c0x14(char p0, char p1);
void c0x15(char p0, int p1);
void c0x16(char p0, char p1);
void c0x17(char p0, char p1);
void c0x18(char p0, char p1);
void Compare(short p0, short p1);
void CompareVars(short p0, short p1);
void CallGlobalScriptAsync(short p0);
void CallGlobalScript(short p0);
void ReturnGlobalScript();
void Jump(int p0);
void If(char p0, int p1);
void c0x20(char p0, int p1);
void c0x21(short p0);
void c0x22(short p0);
void SetFlag(short p0);
void ClearFlag(short p0);
void SetVarFlagStatus(short flagID, short dest);
void AddToVar(short var, short value);
void SubtractVar(short var, short value);
//Sets var equal to the provided value
void SetVarEqVal(short var, short value);
//Sets var1 equal to var2
void SetVarEqVar(short var1, short var2);
//Sets var1 equal to var2
void SetVarEqVar2(short var1, short var2);
void MultiplyVar(short var, short value);
void DivideVar(short var, short value);
void ModuloVar(short var, short value);
void LockAll();
void UnlockAll();
void WaitMoment();
void WaitForABInput();
void WaitForButton();
//Enables or disables whether a message box auto scrolls
void SetMessageAutoscroll(short enabled);
void EventGreyMessage(short lineNumber, short position);
void EventMessageAsync(short lineNumber, short position);
void CloseEventGreyMessage();
void c0x37(short p0);
void BubbleMessage(short lineNumber, char position);
void CloseBubbleMessage();
void ShowMessageAt(short lineNumber, short x, short y, short p3);
void CloseShowMessageAt(short p0);
//Displays a message coming from the provided npc
void Message(short textFile, short lineNumber, short npcID, short p3, short boxType);
//Displays a message coming from the npc the player talked to
void Message2(short textFile, short lineNumber, short p2, short boxType);
void CloseMessageBox();
void CloseAllMessageBoxes();
void MoneyBox(short p0, short p1);
void CloseMoneyBox();
void UpdateMoneyBox();
void BorderedMessage(short p0, short p1);
void CloseBorderedMessage();
void CheckerMessage(short lineNumber, char x, char y, short p3);
void CloseCheckerMessage();
//Displays a Yes or No dialogue option and sends the result to returnVar
void YesNoBox(short returnVar);
//Displays one of two messages depending on the player's gender
void GenderedMessage(short textFile, short maleLineNumber, short femaleLineNumber, short npcID, short p4, short boxType);
//Displays one of two messages depending on the game version
void VersionMessage(short textFile, short whiteLineNumber, short blackLineNumber, short npcID, short p4, short boxType);
void AngryMessage(short p0, char p1);
void WaitMessage();

//Assigns the player's name to a string buffer
void SetWordPlayerName(char stringIndex);
//Assigns an item name to a string buffer
void SetWordItem(char stringIndex, short itemID);
//Assigns a pluralized item name to a string buffer
void SetWordItem2(char stringIndex, short itemID, short itemCount, char p3);
//Assigns an item name with "a" or "an" before it to a string buffer
void SetWordItem3(char stringIndex, short itemID);
//Assigns the move corresponding to a TM to a string buffer
//The item ids for TMs range from 328 to 425
void SetWordTM(char stringIndex, short itemID);
//Assigns a move name to a string buffer
void SetWordMove(char stringIndex, short moveID);
//Assigns the name of a pocket in the bag to a string buffer
void SetWordItemPocket(char stringIndex, short pocketID);
//Assigns the species name of a party pokemon to a string buffer
void SetWordPartyPokemon(char stringIndex, short partySlot);
//Assigns the nickname of a party pokemon to a string buffer
void SetWordPartyNickname(char stringIndex, short partySlot);
//Assigns the species name of a pokemon in the daycare to a string buffer
void SetWordDaycarePokemon(char stringIndex, short daycareSlot);
//Assigns the name of a type to a string buffer
void SetWordType(char stringIndex, short typeID);
//Assigns the species name of a pokemon to a string buffer
void SetWordPokemon(char stringIndex, short pokemonID);
//Assigns the species name of a pokemon with "a" or "an" before it to a string buffer
void SetWordPokemon2(char stringIndex, short pokemonID);
//Assigns the name of a map to a string buffer
void SetWordLocation(char stringIndex, short mapID);
void SetWordPokemonNick(char stringIndex, short p1);
void SetWordDaycareNickname(char stringIndex, short p1);
//Assigns a number to a string buffer
void SetWordNumber(char stringIndex, short value, short maxDigits);
void SetWordMusicalInfo(char stringIndex, char type, short index);
void SetWordCountry(char stringIndex, short countryID);
void SetWordActivities(char stringIndex, short p1);
void SetWordPower(char stringIndex, short p1);
void SetWordTrainerType(char stringIndex, short p1);
void SetWordTrainerType2(char stringIndex, short p1);
void SetWordGeneralWord(char stringIndex, short p1);
void ApplyMovement(short p0, int p1);
void WaitMovement();
void GetNPCMovementCode(short returnVar, short npcID);
void GetNPCPosition(short npcID, short returnX, short returnY);
void GetHeroPosition(short returnX, short returnY);
void CreateNPC(short x, short y, short direction, short npcID, short spriteID, short movementPermission);
void GetNPCSpawnFlag(short npcID, short returnVar);
void AddNPC(short npcID);
void RemoveNPC(short npcID);
void SetNPCPosition(short npcID, short x, short y, short z, short direction);
//Returns the direction the player is facing
void GetPlayerDirection(short returnVar);
void GetNPCInFrontOfPlayer(short returnVar, short successVar);
void c0x70(short p0, short p1, short p2, short p3, short p4);
void c0x71(short p0, short p1, short p2);
void c0x72(short p0, short p1, short p2, short p3);
void c0x73(short p0, short p1);
void FacePlayer();
void PlayerPlaySequence(short sequenceID);
void c0x76(short p0, short p1, short p2, short p3);
void c0x77();
void c0x78(short p0);
void c0x79(short p0, short p1, short p2);
void c0x7A(short p0);
void MoveNpctoCoordinates(short p0, short p1, short p2, short p3);
void c0x7C(short p0, short p1, short p2);
void c0x7D(short p0, short p1, short p2, short p3);
void TeleportUpNPc(short p0);
void c0x7F(short p0, short p1);
void c0x80(short p0);
void c0x81();
void c0x82(short p0, short p1);
void SetVar0x83(short p0);
void SetVar0x84(short p0);
//Initiates a trainer battle with the provided trainers
//Setting trainer2 will result in a double battle, leaving it as 0 with result in a single battle
//Setting canBlackout to 1 will allow the script to continue if the player loses the fight
void StartTrainerBattle(short trainerID, short trainer2ID, short canBlackout);
//Initiates a multi battle with the provided trainers
void StartMultiTrainerBattle(short allyTrainerID, short enemyTrainerID, short enemyTrainerID2, short canBlackout);
void c0x87(short p0, short p1, short p2);
void c0x88(short p0, short p1, short p2);
void c0x8A(short p0, short p1);
void PlayTrainerMusic(short trainerID);
//Blackout sequence from losing a trainer battle
void TrainerBattleLose();
//Returns 1 if the player won the last trainer battle, 0 if the player lost
void GetTrainerBattleResult(short returnVar);
//Restore the overworld scene after a trainer battle
void TrainerBattleEnd();
void c0x8F(short p0);
void dvar90(short p0);
void dvar92(short p0, short p1);
void dvar93(short p0, short p1);
void TradedTrainerBattle(short p0, short p1, short p2, short p3);
void DeactiveTrainerId(short trainerID);
void ReactiveTrainerId(short trainerID);
void GetTrainerIDActive(short trainerID, short returnVar);
void ChangeMusic(short p0);
void c0x9B(short p0, short p1);
void FadeToDefaultMusic();
void c0x9F(short p0);
void c0xA0();
void c0xA1(short p0);
void c0xA2(short p0, short p1);
void c0xA3(short p0);
void c0xA4(short p0);
void c0xA5(short p0, short p1);
void PlaySound(short p0);
void WaitSoundA7();
void WaitSound();
void PlayFanfare(short p0);
void WaitFanfare();
void Cry(short p0, short p1);
void WaitCry();
void SetupDialogueSelection2(char xPosition, char yPosition, short defaultIndex, char canCancel, short returnVar);
void SetupDialogueSelection3(char xPosition, char yPosition, short defaultIndex, char canCancel, short returnVar);
//Adds an option for the working multiple choice dialogue box
void AddDialogueOption(short textLine, short hintLine, short returnValue);
//Display the working multiple choice dialogue box
void ShowDialogueSelection();
//Display the working multiple choice dialogue box
void ShowDialogueSelection2();
//Initiates a multiple choice dialigue box
//Should be followed by one or more AddDialogueOptions and a ShowDialogueSelection
//returnVar is set to the return value of the line selected
//If canceled, returnVar is set to some negative value?
void SetupDialogueSelection(char xPosition, char yPosition, short defaultIndex, char canCancel, short returnVar);
void FadeScreen(short p0, short p1, short p2, short p3);
void ResetScreen();
void GiveItem(short itemID, short amount, short successVar);
void TakeItem(short itemID, short amount, short successVar);
void CheckItemBagSpace(short p0, short p1, short p2);
void CheckItemBagNumber(short p0, short p1, short p2);
void StoreItemCount(short p0, short p1);
void c0xBA(short p0, short p1);
void c0xBB(short p0, short p1);
void c0xBC(short p0);
void c0xBD(short p0, short p1);
void Warp(short zoneID, short x, short y, short direction);
void TeleportWarp(short zoneID, short x, short y, short direction);
void RailWarp(short p0, short p1, short p2, short p3, short p4);
void QuicksandWarp(short zoneID, short x, short y);
void MapChangeWarp(short zoneID, short x, short y, short direction);
void UnionWarp();
void TeleportWarp2(short zoneID, short x, short y, short z, short direction);
void UseSurf();
void UseWaterfall(short p0);
void UseCut();
void UseDive(short p0);
void c0xC9();
void c0xCA(short p0);
void StoreRandomNumber(short p0, short p1);
void StoreVarItem(short p0);
void StoreVar0xCD(short p0);
void StoreVar0xCE(short p0);
void StoreVar0xCF(short p0);
void StoreDate(short p0, short p1);
void Store0xD1(short p0, short p1);
void Store0xD2(short p0);
void Store0xD3(short p0);
void StoreBirthDay(short p0, short p1);
void StoreBadge(short p0, short p1);
void GiveBadge(short badgeID);
void GetBadgeCount(short p0);
void c0xD8(short p0, short p1);
void c0xD9();
void c0xDA(short p0, short p1, short p2);
void c0xDB();
void c0xDC(short p0, short p1, short p2, short p3, short p4);
void c0xDD(short p0, short p1);
void SpeciesDisplayDE(short p0, short p1);
void c0xDF(short p0, short p1, short p2);
//Returns 0 for white, 1 for black
void GetVersion(short returnVar);
//Returns 0 for male, 1 for female
void GetHeroGender(short returnVar);
void c0xE2(short p0);
void EnableRunningShoes();
void c0xE4(short p0);
void c0xE5(short p0, short p1);
void c0xE6(short p0, short p1);
void c0xE7(short p0);
void c0xE8(short p0, short p1);
void StoreEA(short p0);
void StoreEB(short p0);
void StoreEC();
void StoreED();
void StoreEE(short p0);
void StoreEF(short p0);
void DaycareDeposit(short partySlot);
void DaycareWithdraw(short daycareSlot);
void DaycareGetSpecies(short returnVar, short daycareSlot);
void DaycareGetForm(short returnVar, short daycareSlot);
void DaycareGetNewLevel(short returnVar, short daycareSlot);
void DaycareGetLevelGain(short returnVar, short daycareSlot);
void DaycareGetWithdrawCost(short returnVar, short daycareSlot);
void RequestPokemonForDayCare(short returnVar);
void DaycareGetGender(short returnVar, short daycareSlot);
void AddMoney(short amount);
void TakeMoney(short amount);
//Returns 1 if the player has the amount of money specified
void CheckMoney(short returnVar, short amount);
void GetPartyFriendship(short returnVar, short partySlot);
//Modifies a pokemon's friendship value based on the provided mode
//0 - set to value
//1 - add value
//2 - subtract value
void ChangePartyFriendship(short partySlot, short mode, short value);
void GetPartySpecies(short returnVar, short partySlot);
void GetPartyForm(short returnVar, short partySlot);
void CheckPokerusInParty(short returnVar);
void GetPartyIsFullHp(short returnVar, short partySlot);
void GetPartyIsEgg(short returnVar, short partySlot);

//Returns the number of pokemon in the players party that match a certain condition
//mode 0 - Number of filled slots
//mode 1 - Number of non egg pokemon
//mode 2 - Number of pokemon that can battle
//mode 3 - Number of eggs
//mode 4 - ???
//mode 5 - Number of empty slots
void GetPartyCount(short returnVar, short mode);
//Fully heals the pokemon in the player's party
void HealPokemon();
void c0x105(short p0, short p1, short p2);
void c0x106(short p0);
void OpenChoosePokemonMenu(short p0, short successVar, short returnVar, short p3);
void c0x108(short p0, short p1);
void c0x109(short p0, short p1, short p2, short p3);
void c0x10A(short p0, short p1, short p2);
void c0x10B(short p0, short p1, short p2);
void GivePokemon(short returnVar, short pokemonID, short form, short level);
void StorePokemonPartyAt(short p0, short p1);
void GivePokemon2(short returnVar, short pokemonID, short form, short level, short ability, short gender, short shiny, short heldItem, short pokeball);
void GiveEgg(short p0, short p1, short p2);
void GetPokemonParam(short returnVar, short partySlot, short paramID);
//stat = 70 + stat enum:
//hp - 0
//attack - 1
//defense - 2
//speed - 3
//sp att - 4
//sp def - 5
void SetPokemonIV(short partySlot, short stat, short amount);
void GetPokemonEVTotal(short returnVar, short partySlot);
void c0x113(short p0, short p1);
void c0x114(short p0, short p1);
void StorePartyCanLearnMove(short p0, short p1, short p2);
void SetVarPartyHasMove(short p0, short p1);
void SetPokemonForm(short partySlot, short form);
void c0x118(short p0, short p1, short p2);
void c0x119(short p0, short p1, short p2);
void c0x11A(short p0, short p1, short p2, short p3);
void StorePartyType(short p0, short p1, short p2);
void c0x11C(short p0, short p1, short p2);
void SetFavorite(short p0);
void c0x11E(short p0);
void c0x11F(short p0, short p1);
void c0x120(short p0, short p1);
void c0x121(short p0, short p1);
void AddBoxPokemon(short successVar, short species, short form, short level);
void AddBoxPokemon2(short successVar, short species, short form, short level, short ability, short gender, short shiny, short heldItem, short pokeball);
void c0x124(short p0, short p1);
void c0x125();
void c0x126(short p0, short p1, short p2, short p3);
void c0x127(short p0, short p1, short p2, short p3);
void c0x128(short p0);
void c0x129(short p0, short p1);
void c0x12A(short p0);
void c0x12B(short p0, short p1, short p2);
void c0x12C(short p0);
void c0x12D(short p0, short p1, short p2, short p3);
void c0x12E(short p0, short p1, short p2);
void c0x12F(short p0);
void BootPCSound();
void PC_131();
void c0x132(short p0);
void c0x134();
//00 Interior
//01 Snow
//02 Rain
//03 Sandstorm
//04 Snowstorm
//05 Hail
//06 Rainstorm (Thundurus)
//07 Rainstorm (Tornadus)
//08 Crystals
//09 Fog (gray)
//0A Fog (dark gray)
//0B Fog (light gray)
//0C Sandstorm (stronger)
//0D Fog (dark gray)
//0E Fog (light gray)
//40 Normal
void SetWeather(short weatherID, short p1);
void c0x137(short p0);
void c0x138(short p0, short p1, short p2);
void c0x139(short p0);
void c0x13A(short p0);
void c0x13B(short p0);
void c0x13C();
void c0x13D();
void c0x13E();
void StartCameraEvent();
void StopCameraEvent();
void LockCamera();
void ReleaseCamera();
void MoveCamera(short pitch, short yaw, int distance, int targetX, int targetY, int targetZ, short interval);
void c0x144(short p0);
void EndCameraEvent();
void c0x146(short p0, short p1);
void ResetCamera(short p0);
void c0x148(short p0, short p1, short p2, short p3, short p4, short p5, short p6, short p7);
void c0x149(short p0, short p1);
void c0x14A();
void c0x14B();
void c0x14C();
void c0x14D(short p0, short p1);
void c0x14E(short p0, short p1, short p2);
void c0x14F(short p0, short p1);
void c0x150(short p0);
void c0x151(short p0, short p1);
void c0x152();
void c0x153();
void PlayCutscene(short p0, short p1);
void c0x155(short p0, short p1);
void c0x156(short p0);
void c0x157(short p0);
void c0x158();
void c0x159(short p0);
void c0x15A();
void c0x15B(short p0);
void c0x15C(short p0);
void c0x15D();
void c0x15E();
void c0x15F();
void c0x160(short p0, short p1);
void c0x161(short p0);
void c0x162();
void c0x163(short p0, char p1);
void c0x164(short p0);
void c0x165(char p0, short p1, short p2);
void c0x166(short p0, char p1, short p2);
void c0x167(short p0, short p1, short p2, short p3);
void c0x168(short p0);
void c0x169(short p0);
void c0x16A(short p0, short p1);
void c0x16E();
void c0x170(short p0);
void SetVar172(short p0);
//Initiates a wild battle with the pokemon provided
void StartWildBattle(short pokemonID, short level, short flags);
//Restore the overworld scene after a wild battle
void WildBattleEnd();
//Blackout sequence from losing a wild battle
void WildBattleLose();
void SetVarBattle177(short p0);
void BattleStoreResult(short p0);
void c0x179();
void c0x17A(short p0);
void c0x17B(short p0, short p1);
void c0x17C(short p0);
void c0x17D(short p0);
void c0x186();
void c0x187(short p0);
void c0x18C(short p0, short p1);
void c0x18D(short p0);
void c0x18E(short p0);
void c0x18F(short p0);
void c0x190(short p0);
void c0x191(short p0);
void c0x197(short p0);
void c0x198(short p0);
void c0x199();
void c0x19A(short p0);
void c0x19B(short p0);
void c0x19C(short p0);
void c0x19D(short p0);
void CallScreenAnimation(short p0);
void c0x1A1(short p0, short p1, short p2, short p3);
void c0x1A2(short p0);
void FadeFromBlack2();
void FadeIntoBlack2();
void FadeFromWhite2();
void FadeIntoWhite2();
void WaitFade();
void c0x1A8();
void DisplaySeasonBanner();
void FadeFromBlack();
void FadeIntoBlack();
void FadeFromWhite();
void FadeIntoWhite();
void c0x1AF(short p0, short p1);
void c0x1B0(short p0, short p1);
void EliteFourLiftWarp();
void c0x1B2(short p0, short p1);
void c0x1B3(short p0, short p1, short p2);
void TradeNPCStart(short p0, short p1);
void TradeNPCQualify(short p0, short p1, short p2);
void c0x1C1(int p0);
void c0x1C2();
void c0x1C3(short p0);
void StoreTrainerID(short p0, short p1);
void c0x1C5(short p0, short p1, short p2, short p3);
void EnableNationalDex();
void GetNationalDexEnabled(short returnVar);
void c0x1C8();
void c0x1C9(short p0, short p1, short p2);
void c0x1CA();
void c0x1CB();
void c0x1CC(short p0);
void c0x1CD(short p0);
void c0x1CE(short p0, short p1);
void c0x1CF(short p0, short p1);
void c0x1D0(short p0, short p1, short p2, short p3);
void c0x1D1(short p0, short p1);
void c0x1D2(short p0, short p1, short p2);
void c0x1D3(short p0, short p1, short p2);
void c0x1D4(short p0, short p1, short p2);
void c0x1D5(short p0, short p1);
void c0x1D6(short p0, short p1);
void c0x1D7(short p0, short p1, short p2, short p3);
void c0x1D8(short p0, short p1, short p2, short p3);
void c0x1D9(short p0, short p1, short p2, short p3, short p4);
void c0x1DA(short p0, short p1, short p2, short p3);
void c0x1DB(short p0, short p1);
void c0x1DC(short p0, short p1);
void c0x1DD(short p0, short p1, short p2);
void c0x1DE(short p0, short p1);
void c0x1DF();
void c0x1E0(short p0, short p1, short p2, short p3);
void c0x1E1();
void c0x1E2();
void c0x1E3(short p0, short p1, short p2);
void c0x1E4(short p0, short p1, short p2, short p3, short p4, short p5);
void c0x1E5();
void c0x1E6();
void c0x1E7();
void c0x1E8(short p0);
void c0x1E9(short p0, short p1, short p2);
void c0x1EA(short p0, short p1);
void c0x1EB(short p0, short p1);
void c0x1EC();
void c0x1ED(short p0);
void c0x1EE(short p0);
void c0x1EF(short p0);
void c0x1F0(short p0);
void c0x1F1(short p0, short p1);
void c0x1F2(short p0);
void c0x1F3(short p0);
void c0x1F4(short p0, short p1);
void c0x1F5(short p0);
void c0x1F6(short p0, short p1, short p2, short p3);
void c0x1F7(short p0, short p1, short p2, short p3);
void c0x1F8(short p0, short p1);
void c0x1F9(short p0, short p1, short p2, short p3);
void c0x1FA(short p0, short p1, short p2, short p3);
void c0x1FB(short p0, short p1);
void c0x1FC(short p0, short p1, short p2);
void c0x1FD(short p0, short p1, short p2, short p3);
void c0x1FE();
void c0x1FF(short p0);
void c0x200(short p0, short p1, short p2);
void c0x201(short p0, short p1);
void c0x202(short p0, short p1);
void c0x203(short p0);
void c0x204(short p0);
void c0x205(short p0);
void c0x206(short p0);
void c0x207(char p0);
void c0x208(char p0);
void c0x209(short p0, short p1);
void c0x20A();
void c0x20B(short p0);
void c0x20C();
void c0x20D(short p0, short p1, short p2, short p3);
void c0x20E(short p0, short p1, short p2, short p3, short p4, short p5);
void c0x20F(short p0, short p1, short p2, short p3);
void c0x210(short p0);
void c0x211(short p0);
void c0x212(short p0, short p1, short p2);
void c0x213(short p0);
void c0x214(short p0, short p1, short p2, short p3);
void c0x215(short p0, short p1);
void c0x216(short p0, short p1);
void c0x217(short p0);
void c0x218(short p0, short p1);
void c0x219(short p0, short p1);
void c0x21A(short p0, short p1);
void c0x21B();
void c0x21C(short p0, short p1);
void c0x21D(short p0);
void c0x21E(short p0);
void c0x21F(short p0, short p1);
void c0x220(short p0, short p1);
void c0x221(short p0, short p1);
void c0x222(short p0);
void GetHiddenPowerType(short returnVar, short partySlot);
//Checks the pokemon's IVs and returns a value based on the provided type
//sum - 0
//max - 1
//hp - 2
//attack - 3
//defense - 4
//speed - 5
//sp att - 6
//sp def - 7
void GetPokemonIV(short partySlot, short type, short returnVar);
void c0x225(short p0);
void c0x226(short p0);
void c0x227(short p0, short p1);
void c0x228(short p0, short p1);
void c0x22A(short p0, short p1);
void c0x22B(short p0, short p1);
void c0x22D(short p0);
void c0x22E(short p0);
void c0x22F(short p0, short p1);
void c0x230(short p0, short p1);
void c0x231(short p0);
void c0x232(short p0, short p1);
void c0x233(short p0);
void c0x234(short p0, short p1, short p2, short p3);
void c0x235(short p0, short p1);
void c0x236(short p0, short p1, short p2, short p3);
void c0x237(short p0, short p1);
void c0x238(short p0);
void c0x239(short p0);
void c0x23A(short p0, short p1);
void c0x23B();
void c0x23C();
void c0x23D();
void c0x23E(short p0, short p1, short p2);
void Close23F();
void c0x240(short p0, short p1);
void c0x241(short p1);
void c0x245(short p0);
void c0x246(short p0);
void c0x247(short p0, short p1, short p2, short p3, short p4);
void c0x248(short p0, short p1, short p2, short p3);
void c0x249(short p0, short p1, short p2, short p3, short p4, short p5);
void c0x24A(short p0);
void c0x24B();
void c0x24C();
void c0x24D();
void c0x24E(short p0, short p1);
void NPCPathFind(short npcID, short targetX, short targetY, short flags, short p4, short pathfindType);
void c0x250(short p0, short p1, short p2, short p3, short p4);
void c0x251(short p0, short p1);
void c0x252(short p0);
void c0x253(char p0);
void c0x25A(short p0);
void c0x25B();
void c0x25C(short p0, short p1, short p2);
void c0x25D(short p0);
void c0x25F(short p0);
void c0x262(short p0, short p1);
void c0x263(short p0);
void c0x264(short p0, short p1);
void c0x265(short p0);
void c0x266(short p0);
void c0x267();
void c0x268(short p0);
void c0x269();
void StoreMedals26C(char p0, short p1);
void StoreMedals26D(char p0, short p1);
void CountMedals26E(char p0, short p1);
void c0x271(short p0, short p1);
void c0x272(short p0, short p1);
void c0x273(short p0);
void c0x274();
void c0x275(char p0, short p1, short p2);
void c0x276(short p0, short p1);
void c0x277();
void c0x278(short p0, short p1, short p2);
void c0x279();
void c0x27A(short p0, short p1);
void c0x27B(short p0, short p1, short p2, short p3);
void c0x27C();
void c0x27D();
void c0x27E();
void c0x27F();
void c0x284(short p0, short p1);
void c0x285(short p0, short p1, short p2);
void c0x28A(short p0, short p1);
void c0x28B();
void c0x28E(short p0);
void c0x28F(short p0);
void SetWordRivalName(char stringIndex);
void c0x291(short p0);
void c0x292(short p0);
void GrottoWarpIn(short grottoID);
void GrottoGetParam(short paramID, short returnVar);
void GrottoReset();
void GrottoWarpOut();
void StartWildBattle2(short pokemonID, short level, short form, short flags);
void c0x298(char p0, short p1);
void SetWordAbility(char stringIndex, short abilityID);
void SetWordNature(char stringIndex, short natureID);
void c0x29B(char p0);
void c0x29E(short p0, short p1);
void c0x29F(short p0);
void StoreHasMedal(short p0, short p1);
void c0x2A1(short p0);
void c0x2A2();
void c0x2A3();
void c0x2A4();
void c0x2A5(short p0);
void c0x2A6();
void c0x2A7(short p0);
void c0x2A8();
void c0x2A9();
void c0x2AC();
void c0x2AE(short p0);
void StoreDifficulty(short p0);
void UnlockKey(short keyID);
void c0x2B1(short p0);
void c0x2B2(short p0, short p1);
void c0x2B3(short p0, short p1);
void c0x2B4(short p0, short p1);
void c0x2B5(short p0, short p1);
void c0x2B6(short p0, short p1);
void c0x2B7(short p0);
void FollowMeStart();
void FollowMeEnd();
void FollowMeCopyStepsTo(short p0);
void c0x2BB();
void c0x2BC(short p0, short p1);
void c0x2BD(short p0);
void c0x2BE(short p0, short p1, short p2, short p3, short p4);
void c0x2C0(short p0, short p1);
void c0x2C2(short p0);
void c0x2C3(short p0);
void c0x2C4();
void c0x2C5(short p0);
void c0x2C6();
void c0x2C7();
void c0x2CA(short p0);
void c0x2CB(short p0);
void c0x2CF(short p0, short p1, short p2, short p3);
void EnableHabitatList();
void c0x2D1(short p0);
void c0x2D2();
void c0x2D3(short p0, short p1);
void c0x2D4(short p0);
void c0x2D5(short p0, short p1);
void c0x2D7(short p0, short p1);
void c0x2D8(short p0);
void c0x2D9(short p0, short p1);
void c0x2DA(short p0);
void c0x2DB(short p0, short p1);
void c0x2DC(short p0, short p1, short p2);
void StoreUnityVisitors(short p0);
void c0x2DE(short p0);
void StoreMyActivities(short p0);
void c0x2E1();
void c0x2E3();
void GrottoSetEncounter(short grottoID, short group, short slot, short gender);
void GrottoCreateEvents();
void c0x2E8(short p0, short p1);
void c0x2E9(short p0, short p1);
void GivePokemon_N(short successVar, short species, short level, short nature, short ability, short gender);
void c0x2ED(short p0, short p1);
void Prop2EE(short p0, short p1);
void c0x2EF(short p0);
void c0x2F0();
void c0x2F1(short p0);
void c0x2F2(short p0, short p1);