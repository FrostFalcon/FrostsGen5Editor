void c0x0();
void c0x1();
void End();
void ReturnAfterDelay(short p0);
void CallRoutine(int p0);
void EndFunction();
void Logic06(short p0);
void Logic07(short p0);
void CompareTo(short p0);
void StoreVar(short p0);
void ClearVar(short p0);
void c0x0B(short p0);
void c0x0C(short p0);
void c0x0D(short p0);
void c0x0E(short p0);
void c0x0F(short p0);
void StoreFlag(short p0);
void Condition(short p0);
void c0x12(short p0, short p1);
void c0x13(short p0, short p1);
void c0x14(short p0);
void c0x15();
void c0x16(short p0);
void c0x17(short p0);
void c0x18();
void Compare(short p0, short p1);
void c0x1A();
void c0x1B();
void CallStd(short p0);
void ReturnStd();
void Jump(int p0);
void If(char p0, int p1);
void c0x20();
void c0x21(short p0);
void c0x22(short p0);
void SetFlag(short p0);
void ClearFlag(short p0);
void SetVarFlagStatus(short flagID, short dest);
void AddToVar(short p0, short p1);
void SubtractVar(short p0, short p1);
void SetVarEqVal(short p0, short p1);
void SetVar29(short p0, short p1);
void SetVar2A(short p0, short p1);
void SetVar2B(short p0);
void c0x2C();
void c0x2D(short p0);
void LockAll();
void UnlockAll();
void WaitMoment();
void c0x31();
void WaitButton();
void MusicalMessage(short p0);
void EventGreyMessage(short p0, short p1);
void CloseMusicalMessage();
void CloseEventGreyMessage();
void c0x37();
void BubbleMessage(short p0, char p1);
void CloseBubbleMessage();
void ShowMessageAt(short p0, short p1, short p2, short p3);
void CloseShowMessageAt(short p0);
void Message(char p0, char p1, short lineNumber, short npcID, short p4, short boxType);
void Message2(char p0, char p1, short lineNumber, short p3, short boxType);
void CloseMessageKeyPress();
void CloseMessageKeyPress2();
void MoneyBox(short p0, short p1);
void CloseMoneyBox();
void UpdateMoneyBox();
void BorderedMessage(short p0, short p1);
void CloseBorderedMessage();
void PaperMessage(short p0, short p1);
void ClosePaperMessage();
void YesNoBox(short p0);
void Message3(char p0, char p1, short p2, short p3, short p4, short p5, short p6);
void DoubleMessage(char p0, char p1, short p2, short p3, short p4, short p5, short p6);
void AngryMessage(short p0, char p1, short p2);
void CloseAngryMessage();
void SetVarHero(char p0);
void SetVarItem(char p0, short p1);
void c0x4E(char p0, short p1, short p2, char p3);
void SetVarItem2(char p0, short p1);
void SetVarItem3(char p0, short p1);
void SetVarMove(char p0, short p1);
void SetVarBag(char p0, short p1);
void SetVarPartyPokemon(char p0, short p1);
void SetVarPartyPokemon2(char p0, short p1);
void SetVar55(char p0, short p1);
void SetVarType(char p0, short p1);
void SetVarPokemon(char p0, short p1);
void SetVarPokemon2(char p0, short p1);
void SetVarLocation(char p0, short p1);
void SetVarPokemonNick(char p0, short p1);
void SetVar5B(char p0, short p1);
void SetVarStoreVal5C(char p0, short p1, short p2);
void SetVarMusicalInfo(short p0, short p1);
void SetVarNations(char p0, short p1);
void SetVarActivities(char p0, short p1);
void SetVarPower(char p0, short p1);
void SetVarTrainerType(char p0, short p1);
void SetVarTrainerType2(char p0, short p1);
void SetVarGeneralWord(char p0, short p1);
void ApplyMovement(short p0, int p1);
void WaitMovement();
void StoreHeroPosition_0x66(short p0, short p1);
void c0x67(short p0);
void StoreHeroPosition(short p0, short p1);
void CreateNPC(short x, short y, short facing, short overworldID, short spriteID, short movementPermission);
void c0x6A(short p0, short p1);
void AddNPC(short p0);
void RemoveNPC(short npcID);
void SetOWPosition(short p0, short p1, short p2, short p3, short p4);
void c0x6E(short p0);
void c0x6F(short p0);
void c0x70(short p0, short p1, short p2, short p3, short p4);
void c0x71(short p0, short p1, short p2);
void c0x72(short p0, short p1, short p2, short p3);
void c0x73(short p0, short p1);
void FacePlayer();
void Release(short p0);
void ReleaseAll();
void Lock(short p0);
void c0x78(short p0);
void c0x79(short p0, short p1, short p2);
void c0x7A();
void MoveNpctoCoordinates(short p0, short p1, short p2, short p3);
void c0x7C(short p0, short p1, short p2, short p3);
void c0x7D(short p0, short p1, short p2, short p3);
void TeleportUpNPc(short p0);
void c0x7F(short p0, short p1);
void c0x80(short p0);
void c0x81();
void c0x82(short p0, short p1);
void SetVar0x83(short p0);
void SetVar0x84(short p0);
void SingleTrainerBattle(short p0, short p1, short p2);
void DoubleTrainerBattle(short p0, short p1, short p2, short p3);
void c0x87(short p0, short p1, short p2);
void c0x88(short p0, short p1, short p2);
void c0x89();
void c0x8A(short p0, short p1);
void PlayTrainerMusic(short p0);
void EndBattle();
void StoreBattleResult(short p0);
void DisableTrainer();
void c0x8F();
void dvar90(short p0, short p1);
void c0x91();
void dvar92(short p0, short p1);
void dvar93(short p0, short p1);
void TrainerBattle(short p0, short p1, short p2, short p3);
void DeactiveTrainerId(short p0);
void c0x96(short p0);
void StoreActiveTrainerId(short p0, short p1);
void ChangeMusic(short p0);
void c0x99();
void c0x9A();
void c0x9B();
void c0x9C();
void c0x9D();
void FadeToDefaultMusic();
void c0x9F(short p0);
void c0xA0();
void c0xA1();
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
void c0xAD();
void c0xAE();
void SetTextScriptMessage(short p0, short p1, short p2);
void CloseMulti();
void c0xB1();
void Multi2(char p0, char p1, char p2, char p3, char p4, short p5);
void FadeScreen(short p0, short p1, short p2, short p3);
void ResetScreen(short p0, short p1, short p2);
void Screen0xB5(short p0, short p1, short p2);
void TakeItem(short p0, short p1, short p2);
void CheckItemBagSpace(short p0, short p1, short p2);
void CheckItemBagNumber(short p0, short p1, short p2);
void StoreItemCount(short p0, short p1);
void c0xBA(short p0, short p1, short p2, short p3);
void c0xBB(short p0, short p1);
void c0xBC(short p0);
void c0xBD();
void Warp(short p0, short p1, short p2);
void TeleportWarp(short p0, short p1, short p2, short p3);
void c0xC0();
void FallWarp(short p0, short p1, short p2);
void FastWarp(short p0, short p1, short p2, short p3);
void UnionWarp();
void TeleportWarp2(short p0, short p1, short p2, short p3, short p4);
void SurfAnimation();
void SpecialAnimation(short p0);
void SpecialAnimation2(short p0, short p1);
void CallAnimation(short p0, short p1);
void c0xC9();
void c0xCA();
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
void SetBadge(short p0);
void StoreBadgeNumber(short p0);
void c0xD8();
void c0xD9();
void c0xDA(short p0, short p1, short p2);
void c0xDB();
void c0xDC();
void c0xDD(short p0, short p1);
void SpeciesDisplayDE(short p0, short p1);
void c0xDF();
void StoreVersion(short p0);
void StoreHeroGender(short p0);
void c0xE2();
void c0xE3();
void c0xE4(short p0);
void StoreE5(short p0);
void c0xE6();
void ActivateRelocator(short p0);
void c0xE8();
void c0xE9();
void StoreEA(short p0);
void StoreEB(short p0);
void StoreEC();
void StoreED();
void StoreEE(short p0);
void StoreEF(short p0);
void c0xF0(short p0, short p1, short p2);
void StoreF1(short p0);
void c0xF2(short p0, short p1);
void c0xF3(short p0, short p1);
void c0xF4(short p0, short p1);
void c0xF5(short p0, short p1);
void c0xF6(short p0, short p1);
void c0xF7(short p0, short p1);
void c0xF8(short p0, short p1);
void c0xF9(short p0);
void TakeMoney(short p0);
void CheckMoney(short p0, short p1);
void StorePartyHappiness(short p0, short p1);
void c0xFD(short p0, short p1, short p2);
void StorePartySpecies(short p0, short p1);
void c0xFF(short p0, short p1);
void c0x100();
void c0x101(short p0, short p1);
void StorePartyNotEgg(short p0, short p1);
void StorePartyCountMore(short p0, short p1);
void HealPokemon();
void c0x105(short p0, short p1, short p2);
void c0x106(short p0);
void OpenChoosePokemonMenu(short p0, short p1, short p2, short p3);
void c0x108(short p0, short p1);
void c0x109(short p0, short p1, short p2, short p3);
void c0x10A(short p0, short p1, short p2);
void c0x10B(short p0, short p1, short p2);
void GivePokemon(short returnVar, short pokemonID, short heldItem, short level);
void StorePokemonPartyAt(short p0, short p1);
void GivePokemon2(short returnVar, short pokemonID, short form, short level, short ability, short gender, short shiny, short heldItem, short pokeball);
void GiveEgg(short p0, short p1, short p2);
void StorePokemonSex(short p0, short p1, short p2);
//stat = 70 + stat enum:
//hp - 0
//att - 1
//def - 2
//spe - 3
//spA - 4
//spD - 5
void SetPokemonIV(short partySlot, short stat, short amount);
void c0x112();
void c0x113(short p0, short p1);
void c0x114(short p0, short p1);
void StorePartyCanLearnMove(short p0, short p1, short p2);
void SetVarPartyHasMove(short p0, short p1);
void VarValDualCompare117(short p0, short p1, short p2, short p3);
void c0x118(short p0, short p1, short p2);
void c0x119();
void c0x11A(short p0, short p1, short p2, short p3);
void StorePartyType(short p0, short p1, short p2);
void c0x11C(short p0, short p1, short p2);
void SetFavorite(short p0);
void c0x11E(short p0);
void c0x11F(short p0, char p1, short p2);
void c0x120(short p0, short p1);
void c0x121(short p0);
void AddBoxPokemon(short successVar, short species, short form, short level);
void AddBoxPokemon2(short successVar, short species, short form, short level, short ability, short gender, short shiny, short heldItem, short pokeball);
void c0x124();
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
void c0x12F();
void BootPCSound();
void PC_131();
void c0x132(short p0);
void c0x133();
void c0x134(short p0, short p1);
void c0x135();
void c0x136(char p0);
void c0x137(short p0);
void c0x138(char p0);
void c0x139(char p0);
void c0x13A(char p0);
void c0x13B(short p0);
void c0x13C();
void c0x13D();
void c0x13E();
void StartCameraEvent();
void StopCameraEvent();
void LockCamera();
void ReleaseCamera();
void MoveCamera(short p0, short p1, short p2, short p3, short p4, short p5, short p6, short p7, short p8, short p9, short p10);
void c0x144(short p0);
void EndCameraEvent();
void c0x146();
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
void c0x157();
void c0x158();
void c0x159(short p0);
void c0x15A(short p0, short p1, short p2, short p3);
void c0x15B(char p0);
void c0x15C(char p0);
void c0x15D();
void c0x15E();
void c0x15F();
void c0x160();
void c0x161();
void c0x162();
void c0x163(short p0, char p1);
void c0x164(char p0);
void c0x165(char p0, short p1, short p2);
void c0x166(char p0, short p1, short p2);
void c0x167(short p0, short p1, short p2, short p3);
void c0x168(short p0);
void c0x169(short p0);
void c0x16A(short p0, short p1);
void PokemonMenuMusicalFunctions(short p0, short p1, short p2, short p3);
void c0x16C(short p0);
void c0x16D();
void c0x16E();
void c0x16F();
void c0x170();
void c0x171();
void SetVar172(short p0);
void c0x173();
void StartWildBattle(short p0, short p1, short p2);
void EndWildBattle();
void WildBattle1();
void SetVarBattle177(short p0);
void BattleStoreResult(short p0);
void c0x179();
void c0x17A(short p0);
void c0x17B(short p0);
void c0x17C(short p0);
void c0x17D(short p0);
void c0x17E(short p0);
void c0x17F(short p0);
void c0x180(short p0);
void c0x181(short p0);
void c0x182(short p0);
void c0x183(short p0);
void c0x184(short p0);
void c0x185(short p0);
void c0x186(short p0);
void c0x187(short p0);
void c0x188(short p0);
void c0x189(short p0);
void c0x18A(short p0);
void c0x18B(short p0);
void c0x18C(short p0, short p1, short p2, short p3);
void c0x18D(short p0);
void c0x18E(short p0);
void c0x18F(short p0);
void c0x190(short p0);
void c0x191(short p0);
void c0x192(short p0);
void c0x193();
void c0x194();
void c0x195();
void c0x196();
void c0x197();
void c0x198();
void c0x199(short p0);
void c0x19A();
void Animate19B(short p0);
void c0x19C(short p0);
void c0x19D();
void c0x19E(short p0);
void CallScreenAnimation(short p0);
void c0x1A0();
void Xtransciever1(short p0, short p1, short p2, short p3);
void c0x1A2();
void FlashBlackInstant();
void Xtransciever4();
void Xtransciever5();
void Xtransciever6(short p0, short p1, short p2);
void Xtransciever7();
void c0x1A8(short p0, short p1, short p2);
void c0x1A9(short p0, short p1, short p2, short p3);
void c0x1AA(short p0, short p1, short p2, short p3);
void FadeFromBlack();
void FadeIntoBlack();
void FadeFromWhite();
void FadeIntoWhite();
void c0x1AF(short p0, short p1);
void c0x1B0();
void E4StatueGoDown();
void c0x1B2();
void c0x1B3();
void TradeNPCStart(short p0, short p1);
void TradeNPCQualify(short p0, short p1, short p2);
void c0x1B6();
void c0x1B7();
void c0x1B8();
void c0x1B9();
void c0x1BA(short p0, short p1);
void c0x1BB();
void c0x1BC();
void c0x1BD(short p0, short p1);
void c0x1BE(short p0, short p1);
void CompareChosenPokemon(short p0, short p1, short p2);
void c0x1C0();
void c0x1C1(short p0, short p1, short p2, short p3, short p4);
void StartEventBC();
void EndEventBC();
void StoreTrainerID(short p0, short p1);
void c0x1C5(short p0);
void StorePokemonCaughtWF(short p0, short p1, short p2);
void c0x1C7(short p0);
void c0x1C8();
void StoreVarMessage(short p0, short p1);
void c0x1CA();
void c0x1CB(short p0, short p1, short p2, short p3, short p4);
void c0x1CC(short p0);
void c0x1CD(short p0);
void c0x1CE(short p0, short p1);
void c0x1CF(short p0);
void c0x1D0(short p0, short p1, short p2, short p3);
void c0x1D1(short p0, short p1);
void c0x1D2(short p0, short p1, short p2);
void c0x1D3(short p0, short p1, short p2);
void c0x1D4(short p0, short p1, short p2);
void c0x1D5(short p0, short p1);
void c0x1D6(short p0, short p1);
void c0x1D7(short p0, short p1, short p2, short p3);
void c0x1D8(short p0, short p1, short p2, short p3);
void c0x1D9(short p0, short p1, short p2, short p3);
void c0x1DA(short p0);
void c0x1DB(short p0);
void c0x1DC(short p0);
void c0x1DD(short p0, short p1, short p2);
void c0x1DE(short p0, short p1);
void c0x1DF();
void c0x1E0(short p0, short p1, short p2, short p3);
void c0x1E1();
void c0x1E2();
void c0x1E3(short p0, short p1, short p2);
void c0x1E4(short p0, short p1, short p2, short p3);
void c0x1E5();
void c0x1E6();
void c0x1E7();
void c0x1E8();
void c0x1E9();
void c0x1EA(short p0, short p1, short p2, short p3);
void c0x1EB();
void SwitchOwPosition(short p0, short p1, short p2, short p3, short p4);
void c0x1ED(short p0, short p1, short p2);
void c0x1EE(short p0, short p1);
void c0x1EF(short p0, short p1);
void c0x1F0(short p0, short p1);
void c0x1F1();
void c0x1F2(short p0);
void c0x1F3(short p0, short p1, short p2, short p3);
void c0x1F4(short p0, short p1);
void c0x1F5();
void c0x1F6(short p0, short p1, short p2, short p3);
void c0x1F7(short p0, short p1, short p2, short p3, short p4, short p5);
void c0x1F8(short p0, short p1);
void c0x1F9();
void c0x1FA();
void c0x1FB(short p0, short p1);
void c0x1FC(short p0, short p1);
void c0x1FD();
void c0x1FE();
void c0x1FF();
void c0x200(short p0);
void c0x201();
void c0x202(short p0);
void c0x203();
void c0x204();
void c0x205(short p0);
void c0x206();
void c0x207(short p0, short p1);
void c0x208(short p0);
void c0x209(short p0, short p1);
void c0x20A(short p0, short p1, short p2, short p3);
void c0x20B(short p0, short p1);
void StorePasswordClown(short p0, short p1, short p2, short p3);
void c0x20D();
void c0x20E(short p0, short p1);
void c0x20F(short p0, short p1, short p2);
void c0x210();
void c0x211();
void c0x212();
void c0x213();
void c0x214(short p0, short p1, short p2, short p3);
void c0x215(short p0, short p1);
void c0x216();
void c0x217(short p0);
void c0x218(short p0, short p1);
void c0x219(short p0, short p1);
void c0x21A(short p0, short p1);
void c0x21B();
void c0x21C(short p0, short p1);
void c0x21D(short p0);
void HipWaderPKMGet(short p0);
void c0x21F(short p0, short p1);
void c0x220(short p0);
void c0x221(short p0, short p1);
void c0x222();
void StoreHiddenPowerType(short p0, short p1);
void c0x224(short p0, short p1, short p2);
void c0x225(short p0);
void c0x226(short p0);
void c0x227(short p0, short p1);
void c0x228();
void c0x229(short p0, short p1);
void c0x22A(short p0);
void c0x22B(short p0, short p1);
void c0x22C(short p0, short p1);
void c0x22D(short p0);
void c0x22E();
void c0x22F(short p0, short p1);
void c0x230(short p0, short p1);
void c0x231(short p0);
void c0x232();
void c0x233(short p0);
void c0x234(short p0, short p1);
void c0x235();
void c0x236(short p0, short p1, short p2, short p3);
void c0x237(short p0, short p1);
void c0x238();
void c0x239(short p0);
void c0x23A(short p0, short p1);
void c0x23B();
void c0x23C();
void c0x23D(short p0, short p1);
void c0x23E(short p0, short p1, short p2);
void Close23F();
void c0x240();
void c0x241();
void c0x242(short p0, short p1);
void c0x243();
void c0x244();
void c0x245(short p0);
void c0x246(short p0);
void c0x247(short p0, short p1, short p2, short p3, short p4);
void c0x248(short p0, short p1);
void c0x249(short p0, short p1, short p2, short p3);
void c0x24A(short p0, short p1);
void c0x24B();
void c0x24C(short p0);
void c0x24D();
void c0x24E(short p0, short p1);
void c0x24F(short p0, short p1, short p2, short p3, short p4, short p5);
void c0x250();
void c0x251(short p0);
void c0x252(short p0);
void c0x253(char p0);
void c0x254(short p0);
void c0x255();
void c0x256();
void c0x257();
void c0x258();
void c0x259();
void c0x25A(short p0);
void c0x25B();
void c0x25C(short p0, short p1, short p2, short p3, short p4, short p5);
void c0x25D();
void c0x25E();
void c0x25F(short p0);
void c0x260();
void c0x261();
void c0x262(short p0, short p1);
void c0x263(short p0);
void c0x264();
void c0x265();
void c0x266(short p0);
void c0x267();
void c0x268();
void c0x269();
void c0x26A();
void c0x26B();
void StoreMedals26C(char p0, short p1);
void StoreMedals26D(char p0, short p1);
void CountMedals26E(char p0, short p1);
void c0x26F();
void c0x270();
void c0x271(short p0, short p1);
void c0x272(short p0, short p1);
void c0x273(short p0);
void c0x274();
void c0x275(char p0, short p1, short p2);
void c0x276(short p0, short p1);
void c0x277();
void c0x278();
void c0x279();
void c0x27A();
void c0x27B();
void c0x27C();
void c0x27D();
void c0x27E();
void c0x27F();
void c0x280();
void c0x281();
void c0x282();
void c0x283(char p0, char p1);
void c0x284(char p0, char p1);
void c0x285(short p0, short p1, short p2);
void c0x286();
void c0x287(short p0, short p1, short p2);
void c0x288(short p0, short p1, short p2);
void c0x289(short p0);
void c0x28A();
void c0x28B();
void c0x28C();
void c0x28D();
void c0x28E();
void c0x28F();
void c0x290(char p0);
void c0x291();
void c0x292(char p0);
void c0x293(char p0);
void c0x294(char p0, char p1);
void c0x295();
void c0x296();
void c0x297(short p0);
void c0x298();
void c0x299();
void c0x29A(char p0, short p1);
void c0x29B(char p0);
void c0x29C();
void c0x29D();
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
void c0x2AA();
void c0x2AB();
void c0x2AC();
void c0x2AD();
void c0x2AE();
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
void c0x2BF();
void c0x2C0(short p0, short p1);
void c0x2C1();
void c0x2C2();
void c0x2C3(short p0, short p1);
void c0x2C4();
void c0x2C5(short p0);
void c0x2C6();
void c0x2C7();
void c0x2C8();
void c0x2C9();
void c0x2CA();
void c0x2CB(short p0);
void c0x2CC();
void c0x2CD();
void c0x2CE();
void c0x2CF(short p0, short p1, short p2, short p3);
void EnableHabitatList();
void c0x2D1(short p0);
void c0x2D2();
void c0x2D3();
void c0x2D4(short p0);
void c0x2D5(short p0, short p1);
void c0x2D6();
void c0x2D7(short p0, short p1);
void c0x2D8();
void c0x2D9(short p0, short p1);
void c0x2DA(short p0);
void c0x2DB(short p0, short p1);
void c0x2DC(short p0, short p1, short p2);
void StoreUnityVisitors(short p0);
void c0x2DE();
void StoreMyActivities(short p0);
void c0x2E0();
void c0x2E1();
void c0x2E2();
void c0x2E3();
void c0x2E4();
void c0x2E5();
void c0x2E6();
void c0x2E7();
void c0x2E8(short p0, short p1);
void c0x2E9();
void GivePokemon_N(short p0, short p1, short p2, short p3, short p4, short p5);
void c0x2EB();
void c0x2EC();
void c0x2ED(short p0, short p1);
void Prop2EE(short p0, short p1);
void c0x2EF(short p0);
void c0x2F0();
void c0x2F1(short p0);
void c0x2F2();
void c0x2F3();
void c0x2F4();
void c0x2F5();
void c0x2F6();
void c0x2F7();
void c0x2F8();
void c0x2F9();
void c0x2FA();
void c0x2FB();
void c0x2FC();
void c0x2FD();
void c0x2FE();
void c0x2FF();
void c0x300();
void c0x301();
void c0x302();
void c0x303();
void c0x304();
void c0x305();
void c0x306();
void c0x307();
void c0x308();
void c0x309();
void c0x30A();
void c0x30B();
void c0x30C();
void c0x30D();
void c0x30E();
void c0x30F();
void c0x310();
void c0x311();
void c0x312();
void c0x313();
void c0x314();
void c0x315();
void c0x316();
void c0x317();
void c0x318();
void c0x319();
void c0x31A();
void c0x31B();
void c0x31C();
void c0x31D();
void c0x31E();
void c0x31F();
void c0x320();
void c0x321();
void c0x322();
void c0x323();
void c0x324();
void c0x325();
void c0x326();
void c0x327();
void c0x328();
void c0x329();
void c0x32A();
void c0x32B();
void c0x32C();
void c0x32D();
void c0x32E();
void c0x32F();
void c0x330();
void c0x331();
void c0x332();
void c0x333();
void c0x334();
void c0x335();
void c0x336();
void c0x337();
void c0x338();
void c0x339();
void c0x33A();
void c0x33B();
void c0x33C();
void c0x33D();
void c0x33E();
void c0x33F();
void c0x340();
void c0x341();
void c0x342();
void c0x343();
void c0x344();
void c0x345();
void c0x346();
void c0x347();
void c0x348();
void c0x349();
void c0x34A();
void c0x34B();
void c0x34C();
void c0x34D();
void c0x34E();
void c0x34F();
void c0x3F6(short p0);
void c0x3F9();
void c0x3FA(short p0);
void c0x3FC(short p0);
void c0x3FD();
void c0x3FE(short p0, short p1);
void c0x401(short p0);
void c0x402(short p0);
void c0x403(short p0, short p1);
void c0x404(short p0);
void c0x406(short p0, short p1);
void c0x407(short p0, short p1);
void c0x40D(short p0);
void c0x40E(short p0);
void c0x410(short p0);
void c0x411(short p0);
void c0x412(short p0);
void c0x414(char p0, short p1);
void c0x415(char p0);
void c0x416(short p0);
void c0x417(short p0);
void c0x418(short p0);
void c0x419(short p0, short p1);
void c0x41A(short p0);
void c0x41B(short p0, short p1);
void c0x41C(short p0);
void c0x41F(short p0, short p1);
void c0x420(short p0);
void c0x421(short p0, short p1);
void c0x422(short p0);