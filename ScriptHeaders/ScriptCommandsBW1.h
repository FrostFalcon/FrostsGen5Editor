//Does nothing.
void VMNop();
//Does nothing.
void VMNop2();
//Ends a script.
void VMHalt();
//Pauses execution for num frames. (30 frames in the overworld = 1 second).
void VMSleep(short num);
//Jumps to a relative script branch and transfers the program counter to it.
void VMCall(int addr);
//Ends the current script branch and returns the program counter to the original script (?).
void VMReturn();
//TODO
void DebugPrint(short unused);
//TODO
void DebugStack(short unused);
//Pushes a variable onto the stack.
void StackPushConst(short value);
//Pushes a variable's value onto the stack.
void StackPush(short var);
//Removes a variable from the stack.
void StackPop(short dest);
//Clears the stack (?).
void StackDiscard();
//Adds two variables on the stack.
void StackAdd();
//Subtracts two variables on the stack.
void StackSub();
//Multiplies two variables on the stack.
void StackMul();
//Divides two variables on the stack.
void StackDiv();
//Stores the flag on the stack(?).
void StackPushFlag(short flag);
//Compares the two topmost elements of the stack based on a condition and pushes the result back onto the stack.
// == - 1
// != - 5
// > - 2
// >= - 4
// < - 0
// <= - 3
void StackCmp(short cond);
//var &= operand
void WorkAnd(short var, short operand);
//var |= operand
void WorkOr(short var, short operand);
//Stores an 8-bit integer in a global variable (referenced by id).
void VMRegSet8(char dstReg, char val);
//Stores a 32-bit integer in a global variable (referenced by id).
void VMRegSet32(char dstReg, int val);
//TODO
void VMRegMov(char dstReg, char srcReg);
//Compares the values of two global variables (referenced by id).
void VMRegCmp8(char lhsReg, char rhsReg);
//Compares the values of a global variable and a user supplied variable.
void VMRegCmpConst8(char reg, char value);
//Compares the values of a variable and a user supplied number.
void WorkCmpConst(short var, short value);
//Compares the values of two variables.
void WorkCmpWork(short lhsVar, short rhsVar);
//Calls a common script alongside the currently running script.
void RTCallGlobalAsync(short scrId);
//Calls a common script and waits until it completes.
void RTCallGlobal(short scrId);
//Stops the standard function.
void RTEndGlobal();
//Jumps to and sets the program counter to a relative script branch (whose distance is specified in addr).
void VMJump(int addr);
//Jumps to and sets the program counter to a relative script branch (whose distance is specified in addr), if the given condition is true.
void VMJumpIf(char cond, int addr);
//Jumps to but doesn't set the program counter to a relative script branch (whose distance is specified in addr), if the given condition is true.
void VMCallIf(char cond, int addr);
//Reserves the next executed SCRID to a constant value. Can be used across a warp as well to start another script automatically.
void RTReserveScript(short scrId);
//TODO
void FieldGetContinueFlag(short dest);
//Sets a given system flag to TRUE (1).
void FlagSet(short flag);
//Clears the state of a given system flag.
void FlagReset(short flag);
//Reads the value of a flag, and stores it in var.
void FlagGet(short flag, short dest);
//Accesses the second variable, grabs the value of the variable, then adds it to the value in the first variable.
void WorkAdd(short var, short operand);
//Accesses the second variable, grabs the value of the variable, then subtracts it from the value in the first variable.
void WorkSub(short var, short operand);
//Stores val in the specified variable.
void WorkSetConst(short var, short value);
//Copies the value in src to the variable dest. Also known as: var.value = src.value
void WorkGet(short dest, short src);
//Copies the value in src to the variable dest. Also known as: var.value = src.value (?)
void WorkSet(short dst, short src);
//Multiplies the value of var by val. Also known as: var.value *= val
void WorkMul(short var, short operand);
//Divides the value of var by val. Also known as: var.value /= val
void WorkDiv(short var, short operand);
//Divides the value of var by val and returns the remainder of the operation. Also known as: var.value %= val
void WorkMod(short var, short operand);
//Freezes all NPCs.
void ActorPauseAll();
//Unfreezes all NPCs.
void ActorUnlockAll();
//TODO
void RTFinishSubEvents();
//Waits until "A" or "B" is pressed.
void InputWaitAB();
//Waits until any key is pressed.
void InputWaitLast();
//Opens a message with music.
void MsgSetAutoscrolls(short enabled);
//Opens an event-gray message box.
void MsgSystem(short msgId, short pos);
//Opens another event-gray message box.
void MsgSystemAsync(short msgId, short pos);
//Closes an event-gray message box.
void MsgSystemClose();
//Shows/hides a loading spinner at the current message box.
void MsgSetLoadingSpinner(short hide);
//Opens a message which isn't associated with an NPC.
void MsgInfo(short msgId, char pos);
//Closes the message which isn't associated with an NPC.
void MsgInfoClose();
//Opens a message at position (x, y). The coordinates are based on screen hardware tiles. 
void MsgMulti(short msgId, short x, short y, short handle);
//Closes the positional message.
void MsgWinCloseNo(short handle);
//Prints a message with a tail for an NPC. The NPC is manually determined.
void MsgActorEx(short textFile, short msgId, short actorId, short pos, short type);
//Prints a message with a tail for an NPC. The NPC is automatically determined.
void MsgActor(short textFile, short msgId, short pos, short type);
//Closes a message when a key is pressed.
void MsgActorClose();
//Closes a message when a key is pressed. Not sure what makes this different just yet.
void MsgWinCloseAll();
//Shows the money box at coords (x, y).
void MoneyWinDisp(short x, short y);
//Closes the money box.
void MoneyWinClose();
//Updates the values in the money box.
void MoneyWinUpdate();
//Opens a message with a border.
void MsgPlaceSign(short msgId, short type);
//Closes the bordered message.
void MsgPlaceSignClose();
//Opens a message with a checkerboard background.
void MsgCheckerBG(short msgId, char posX, char posY, short unused);
//Closes the checkerboard message.
void MsgCheckerBGClose();
//Opens a box with two options, yes or no. The result of the choice made by the player is stored in var.
void YesNoWin(short dest);
//A message which changes depending on the player's gender.
void MsgActorGendered(short textFile, short msgIdM, short msgIdF, short actorId, short pos, short type);
//A message which changes depending on whether the game is Black or White.
void MsgActorVersioned(short textFile, short msgIdW, short msgIdB, short actorId, short pos, short type);
//Opens a message with a spiked border. Known as the angry message by some.
void MsgScream(short id, char pos);
//Closes all messages on screen.
void MsgWaitAdvance();
//TODO
void WordSetPlayerName(char strbufIdx);
//TODO
void WordSetItemName(char strbufIdx, short itemId);
//TODO
void WordSetItemNameEx(char strbufIdx, short itemId, short amount, char article);
//TODO
void WordSetItemNameWithArticle(char strbufIdx, short itemId);
//TODO
void WordSetTMMoveName(char strbufIdx, short itemId);
//TODO
void WordSetMoveName(char strbufIdx, short moveId);
//TODO
void WordSetItemPocketName(char strbufIdx, short itemId);
//TODO
void WordSetPartyPokeSpecies(char strbufIdx, short partyIdx);
//TODO
void WordSetPartyPokeName(char strbufIdx, short partyIdx);
//TODO
void WordSetDaycarePokeSpecies(char strbufIdx, short daycareSlot);
//TODO
void WordSetPokeTypeName(char strbufIdx, short typeId);
//TODO
void WordSetPokeSpecies(char strbufIdx, short species);
//TODO
void WordSetPokeSpeciesWithArticle(char strbufIdx, short species);
//TODO
void WordSetPlaceName(char strbufIdx, short zoneId);
//TODO
void WordSetTrendName(char strbufIdx, short trendId);
//TODO
void WordSetDaycarePokeName(char strbufIdx, short species);
//TODO
void WordSetNumber(char strbufIdx, short value, short magnitude);
//TODO
void WordSetMusicalInfo(char strbufIdx, char type, short index);
//TODO
void WordSetCountry(char strbufIdx, short countryId);
//TODO
void WordSetHobbyName(char strbufIdx, short hobbyId);
//TODO
void WordSetPassPowerName(char strbufIdx, short passPowerId);
//TODO
void WordSetTrainerClassName(char strbufIdx, short classId);
//TODO
void WordSetTrainerClassNameWithArticle(char strbufIdx, short classId);
//TODO
void WordSetSurveyAnswer(char strbufIdx, short answerIdx);
//Makes the NPC with the ID $id perform a series of actions, relatively located at $addr.
void ActorCmdExec(short actorId, int addr);
//Waits for any NPC movement to stop before continuing script execution.
void ActorCmdWait();
//Gets an Actor's MoveCode.
void ActorGetMoveCode(short dest, short actorId);
//Stores an Actor's XY grid location to two variables.
void ActorGetGPos(short actorId, short pX, short pZ);
//Stores the player's XY grid location to two variables.
void PlayerGetGPos(short pX, short pZ);
//Spawns a new NPC with $id and $objcode at $x $y $z with MoveCode $moveCode.
void ActorNew(short x, short z, short dir, short actorId, short objCode, short moveCode);
//TODO
void ActorGetSpawnFlag(short actorId, short dest);
//Shows the hidden NPC with the id ID.
void ActorAdd(short actorId);
//Hides the NPC with the id ID.
void ActorDelete(short actorId);
//Sets an Actor's grid position and direction.
void ActorSetGPos(short actorId, short x, short y, short z, short dir);
//Gets the direction of the player.
void PlayerGetDir(short dest);
//TODO
void PlayerGetActorInFront(short pActorId, short pSuccess);
//TODO
void ActorFindByGPos(short pActorId, short pSuccess, short x, short y, short z);
//Gets the player's rail position.
void PlayerGetRailPos(short pLineId, short pPosFront, short pPosSide);
//Gets an Actor's rail position. Behavior is undefined if the actor is on grid.
void ActorGetRailPos(short actorId, short pLineId, short pPosFront, short pPosSide);
//Sets an Actor's MoveCode.
void ActorSetMoveCode(short actorId, short moveCode);
//Causes both the player and the NPC that this script is assigned to face each other.
void ActorSetEyeToEye();
//TODO
void PlayerSetSpecialSequence(short seqId);
//TODO
void PlayerMoveToYAsync(short direction, short interval, short destY, short isYPositive);
//If the player is currently standing in the area of a trigger with a direction value set, they will turn in that direction.
void PlayerTurnByTrigger();
//TODO
void PlayerGetExState(short dest);
//TODO
void ActorGetUserParam(short actorId, short paramIdx, short dest);
//Overlay 131 - rail_slipdown.c
void ActorPlayRailSlipdown(short actorId);
//Moves NPC to specified XYZ coordinates on map (in that order).
void ActorJumpToGPos(short actorId, short x, short y, short z);
//TODO
void PlayerSetRailPos(short lineId, short posFront, short posSide);
//TODO
void ActorSetRailPos(short actorId, short lineId, short posFront, short posSide);
//TODO
void ActorPlayTeleportSeq(short actorId);
//TODO
void TrainerEyeGetTrainerID(short trSlotId, short dest);
//TODO
void TrainerEyeEventInit(short trSlotId);
//TODO
void TrainerEyeEventStart();
//TODO
void TrainerEyeGetActorID(short trSlotId, short dest);
//TODO
void TrainerEyeGetBattleType(short dest);
//TODO
void ActorGetTrainerID(short dest);
//TODO
void CallTrainerBattle(short trId1, short trId2, short flags);
//Begins a Multi Battle against two Trainers specified by params 2 and 3, with an ally specified by param 1. Win/loss logic is decided by parameter 4.
void CallTrainerMultiBattle(short allyId, short trId1, short trId2, short flags);
//TODO
void TrainerEyeSayMessage(short trSlotId, short msgType, short actorId);
//TODO
void TrainerEyeGetMessageTypes(short pIdBeforeBtl, short pIdDefeated, short pIdReject);
//TODO
void TrainerGetBattleType(short trId, short dest);
//TODO
void TrainerBGMPlayPush(short trId);
//Calls the blackout sequence after the player is defeated in a Trainer battle.
void CallTrainerLose();
//TODO
void TrainerBattleIsVictory(short dest);
//TODO
void CallTrainerBattleEnd();
//TODO
void TrainerGetFieldAction(short trId, short dest);
//TODO
void TrainerGetPrizeItem(short trId, short dest);
//Starts a trainer battle using a previously traded Pokémon as the opponent.
void CallTradedPokemonBattle(short trId1, short trId2, short flags, short savePkmIdx);
//TODO
void TrainerFlagSet(short trId);
//TODO
void TrainerFlagReset(short trId);
//TODO
void TrainerFlagGet(short trId, short dest);
//Plays a music track corresponding to id in swan_sound_data.sdat.
void BGMPlay(short sndId);
//TODO
void BGMIsPlaying(short sndId, short dest);
//Fades out currently playing BGM track and plays the track assigned to the current map.
void BGMChangeMap();
//TODO
void BGMPlayPush(short sndId);
//TODO
void BGMWait();
//TODO
void BGMPush(short fadeOutTime);
//TODO
void BGMPop(short fadeOutTime, short fadeInTime);
//TODO
void ISSSwitchEnable(short switchIndex);
//TODO
void ISSSwitchDisable(short switchIndex);
//TODO
void ISSSwitchQuery(short dest, short switchIndex);
//Plays a sound effect corresponding to id in swan_sound_data.sdat.
void SEPlay(short sndId);
//Stops a currently playing sound effect.
void SEStop();
//Pauses execution until the sound effect ends.
void SEWait();
//Plays a fanfare corresponding to id in swan_sound_data.sdat.
void MEPlay(short sndId);
//Pauses execution until the fanfare being played ends.
void MEWait();
//Plays a cry corresponding to id in swan_sound_data.sdat.
void PVPlay(short species, short forme);
//Pauses execution until the cry being played ends.
void PVWait();
//Uses common text file 410 instead of zonedata text.
void ListMenuInitCommon(char x, char y, short defaultIdx, char cancellable, short pDlgResult);
//TODO
void ListMenuInitTL(char x, char y, short defaultIdx, char cancellable, short pDlgResult);
//TODO
void ListMenuAdd(short optionMsgId, short hintMsgId, short uid);
//TODO
void ListMenuShow();
//TODO
void ListMenuShow2();
//TODO
void ListMenuInitTR(char x, char y, short defaultIdx, char cancellable, short pDlgResult);
//Mode - 1=Engine A/Black, 2=Engine B/Black, 4=Engine A/White, 8=Engine B/White. Can be ORed together. Slowness = amount of frames between each fade tick. Negative values allow for more fade ticks per update, but are not attainable in the unsigned scripting paradigma.
void FadeEx(short mode, short brightnessStart, short brightnessEnd, short slowness);
//TODO
void FadeExWait();
//TODO
void ItemAdd(short itemId, short amount, short pSuccess);
//TODO
void ItemSub(short itemId, short amount, short pSuccess);
//TODO
void ItemCheckSpace(short itemId, short amount, short pSuccess);
//TODO
void ItemCheckCount(short itemId, short amount, short pSuccess);
//TODO
void ItemGetCount(short itemId, short dest);
//TODO
void ItemIsTMHM(short itemId, short dest);
//TODO
void ItemGetPocket(short itemId, short dest);
//TODO
void PhenomenonGetItemID(short dest);
//TODO
void ItemGetClass(short item, short dest);
//A warp. Seems to detach followers, however.
void MapChangeFake(short zoneId, short x, short z, short dir);
//TODO
void MapChangeWarpPad(short zoneId, short x, short z, short dir);
//TODO
void MapChangeWarpRail(short zoneId, short lineId, short posFront, short posSide, short dir);
//TODO
void MapChangeQuicksand(short zoneId, short x, short z);
//The warp that's generally used when in a script
void MapChangeWarp(short zoneId, short x, short z, short dir);
//TODO
void MapChangeUnionRoom();
//TODO
void MapChangeCore(short zoneId, short x, short y, short z, short dir);
//TODO
void HMCallSurf();
//TODO
void HMCallWaterfall(short partyIdx);
//TODO
void HMCallCut();
//TODO
void HMCallDiving(short isOut);
//TODO
void CMD_C9();
//TODO
void CMD_CA(short p0);
//TODO
void Random(short dest, short bound);
//TODO
void RTGetTextFile(short dest);
//TODO
void RTCGetDayPart(short dest);
//TODO
void CMD_CE(short p0);
//TODO
void RTCGetWeekDay(short dest);
//TODO
void RTCGetDate(short pMonth, short pDay);
//TODO
void RTCGetTime(short pHour, short pMinute);
//TODO
void RTCGetSeason(short dest);
//TODO
void FieldGetZoneID(short dest);
//TODO
void TrainerCardGetBirthDate(short pMonth, short pDay);
//TODO
void TrainerCardHasBadge(short dest, short badgeNo);
//TODO
void TrainerCardAddBadge(short badgeNo);
//TODO
void TrainerCardGetBadgeCount(short dest);
//TODO
void MapReplaceIsEventSet(short id, short dest);
//TODO
void FieldSetTeleportZone(short tpZoneSlot);
//TODO
void MapReplaceSetEvent(short id, short isEnabled, short doReload);
//TODO
void FieldSetNextZoneHere();
//TODO
void FieldSetNextZone(short zoneId, short dir, short x, short y, short z);
//TODO
void PokeDexGetCount(short caughtOnly, short dest);
//TODO
void PokeDexRegist(short mode, short species);
//TODO
void PokeDexIsRegist(short caughtOnly, short species, short dest);
//TODO
void GameGetVersion(short dest);
//TODO
void TrainerCardGetSex(short dest);
//TODO
void SaveDataCheckRequired(short dest);
//TODO
void PlayerEnableRunningShoes();
//TODO
void CMD_E4(short p0);
//TODO
void CMD_E5(short p0, short p1);
//TODO
void CMD_E6(short p0, short p1);
//TODO
void ActivateRelocator(short p0);
//TODO
void CMD_E8(short p0, short p1);
//TODO
void CMD_E9(short p0);
//TODO
void HOFCheckIntegrity(short dest);
//TODO
void DayCareCheckSpawnFlag(short dest);
//TODO
void DayCareBreed();
//TODO
void DayCareResetSeed();
//TODO
void DayCareGetPkmCount(short dest);
//TODO
void DayCareCalcEggSpawnChance(short dest);
//TODO
void DayCareDeposit(short partyIdx);
//TODO
void DayCareWithdraw(short daycareSlot);
//TODO
void DayCareGetSpecies(short dest, short daycareSlot);
//TODO
void DayCareGetForme(short dest, short daycareSlot);
//TODO
void DayCareCalcNewLevel(short dest, short daycareSlot);
//TODO
void DayCareCalcLevelGain(short dest, short daycareSlot);
//TODO
void DayCareCalcWithdrawCost(short dest, short daycareSlot);
//TODO
void DayCareCallPokeSelect(short dest);
//TODO
void DayCareGetSex(short dest, short daycareSlot);
//TODO
void MoneyAdd(short amount);
//TODO
void MoneySub(short amount);
//Checks if the player has a sufficient amount of money.
void MoneyCheck(short dest, short neededAmount);
//TODO
void PokePartySetHappiness(short dest, short partyIdx);
//Manipulates a Pokémon's Happiness value. Mode 0/1/2 means Set/Add/Subtract.
void PokePartyAdjustHappiness(short partyIdx, short mode, short value);
//TODO
void PokePartyGetSpecies(short dest, short partyIdx);
//TODO
void PokePartyGetForme(short dest, short partyIdx);
//TODO
void PokePartyCheckPokerus(short dest);
//TODO
void PokePartyIsFullHP(short dest, short partyIdx);
//TODO
void PokePartyIsEgg(short dest, short partyIdx);
//Returns the number of Pokémon in the party that match a certain conditon. (All/NonEgg/Battling-capable/Eggs/SanityEggs)
void PokePartyGetCount(short dest, short mode);
//Fully heals the player's party.
void PokePartyRecoverAll();
//Pops up a dialog for renaming a party Pokémon and stores its result.
void CallPokeNameInput(short pSuccess, short poke, short transition);
//TODO
void CallEggHatch(short pPartyIdx);
//TODO
void CallPokeSelect(short wantEgg, short pDlgSuccess, short pPartyIdx);
//TODO
void PokePartyGetMoveCount(short dest, short partyIdx);
//TODO
void CallPokeMoveReplace(short pDlgSuccess, short pForgetMoveIdx, short partyIdx, short newMoveId);
//TODO
void PokePartyGetMove(short dest, short partyIdx, short moveId);
//Teaches a move to a Pokémon. (wazaID == 0 > unlearn)
void PokePartyLearnMove(short partyIdx, short moveIdx, short moveId);
//A convenience method for AddPoke.
void PokePartyAdd(short pSuccess, short species, short forme, short level);
//Returns the index of the first Pokémon that is either an egg (1) or fainted (2).
void PokePartyGetMemberByType(short dest, short cond);
//Adds a Pokémon to the player's party. Fails if the party is full.
void PokePartyAddEx(short pSuccess, short species, short forme, short level, short abilChoice, short gender, short isShiny, short heldItem, short ball);
//Adds a Pokémon Egg to the player's party. Fails if the party is full.
void PokePartyAddEgg(short pSuccess, short species, short forme);
//TODO
void PokePartyGetParam(short dest, short partyIdx, short paramId);
//Sets the IV of a Pokémon to a given value. The IV field is 70 to 75.
void PokePartySetIV(short partyIdx, short paramId, short value);
//Returns the sum of all EVs of a party Pokémon.
void PokePartyGetEVTotal(short dest, short partyIdx);
//Checks if the party Pokémon belongs to the player and was caught in this game version.
void PokePartyIsOriginGame(short dest, short partyIdx);
//TODO
void PokePartyGetCountBySpecies(short species, short dest);
//Checks if a party Pokémon knows a given move.
void PokePartyHasMove(short dest, short moveId, short partyIdx);
//Checks if any party Pokémon knows a given move.
void PokePartyHasMoveAny(short dest, short moveId);
//Changes a party Pokémon's forme.
void PokePartySetForme(short poke, short forme);
//Gets the index of the first Pokémon of a species in the party.
void PokePartyFind(short species, short pSuccess, short pPartyIdx);
//TODO
void PokePartyIsFromWhiteForest(short dest, short poke, short locIndex);
//TODO
void PokePartyGetMetDate(short pYear, short pMonth, short pDay, short partyIdx);
//TODO
void PokePartyGetTypes(short pType1, short pType2, short partyIdx);
//TODO
void PokePartyChangeRotomForme(short partyIdx, short signatureMoveSlot, short forme);
//TODO
void TrainerCardSetFavePokemon(short partyIdx);
//TODO
void TrainerCardSaveGymVictoryParty(short badgeNo);
//TODO
void WordSetGymVictoryParty(short badgeNo, short pPartyCount);
//TODO
void FieldTradeSavePokemon(short partyIdx, short savePkmIdx);
//Gets the number of Pokémon in the PC box that match a certain criteria. (0/1/2/3/4/5=All/Normal/Ditto/Non-normal/Free slots/0)
void BoxGetCount(short dest, short mode);
//Adds a Pokémon to the Pokémon Storage System Box and registers it in the Pokédex. Fails if box is full (cap at 720).
void BoxAdd(short pSuccess, short species, short forme, short level);
//TODO
void BoxAddEx(short pSuccess, short species, short forme, short level, short abilChoice, short gender, short isShiny, short heldItem, short ball);
//TODO
void BMHndAnmPlay(short handle, short anmIdx);
//TODO
void BMPlayHOFMachineSeq();
//TODO
void BMChangeMdlID(short bmType, short x, short z, short mdlId);
//TODO
void BMCreateHandleByGPos(short dest, short bmType, short x, short z);
//TODO
void BMReleaseHandle(short handle);
//TODO
void BMHndAudioVisualAnmPlay(short handle, short anmIdx);
//TODO
void BMHndAnmWait(short handle);
//TODO
void BMAnmPlayInv(short bmType, short x, short z);
//TODO
void BMHndAnmPause(short handle);
//TODO
void BMSetVisible(short bmType, short x, short z, short visible);
//TODO
void BMAnmPlay(short bmType, short x, short z);
//TODO
void PokecenPlayHealingSequence(short healPkmCount);
//TODO
void PokecenPCOpen();
//TODO
void PokecenPCIdle();
//TODO
void PokecenPCClose(short skipWaitForSfx);
//TODO
void CasteliaRushInit();
//TODO
void FieldSetWeather(short type);
//TODO
void SaveDataWrite(short pSuccess);
//TODO
void SaveDataGetStatus(short pAnotherSavePresent, short pSaveStatus, short pLotsOfData);
//TODO
void GameCommDisconnect(short pResult);
//TODO
void GameCommGetStatus(short dest);
//TODO
void GameCommCheckDSiWiFi(short dest);
//TODO
void CMD_13b(short p0);
//TODO
void CMD_13c();
//TODO
void CMD_13d();
//TODO
void CMD_13e();
//TODO
void EVCameraInit();
//TODO
void EVCameraEnd();
//TODO
void EVCameraUnbind();
//TODO
void EVCameraRebind();
//TODO
void EVCameraMoveTo(short pitch, short yaw, int distance, int targetX, int targetY, int targetZ, short interval);
//TODO
void EVCameraReturn(short interval);
//TODO
void EVCameraWait();
//TODO
void EVCameraMoveToCommon(short id, short interval);
//TODO
void EVCameraMoveToDefault(short interval);
//TODO
void EVCameraShake(short intensityH, short intensityW, short loopLength, short loopCount, short decayX, short decayY, short decayStartLoop, short decayStepLength);
//TODO
void CallFriendlyShopBuy(short shopId, short unused);
//TODO
void FieldOpen();
//TODO
void FieldClose();
//TODO
void RTFreeUserHeap();
//TODO
void CallRecordSystem(short mode, short pForceQuit);
//TODO
void CallBag(short mode, short pSuccess, short pSelectedItem);
//TODO
void CallPC(short pForceQuit, short mode);
//TODO
void CallMailbox(short pForceQuit);
//TODO
void CallPokedexDiploma(short diplomaId, short soundOff);
//TODO
void CallGeonet();
//TODO
void CallPoke3Select(short dest);
//TODO
void Call3DDemo(short seqId, short userParam);
//TODO
void CallXTransceiver(short sceneId, short p1);
//TODO
void CallGameClear(short repeated);
//TODO
void CMD_157();
//TODO
void CMD_158(short p0, short p1);
//TODO
void CMD_159();
//TODO
void CMD_15a(short p0, short p1, short p2, short p3);
//TODO
void CMD_15b(short p0);
//TODO
void CMD_15c();
//TODO
void NetConnectWiFiClub();
//TODO
void NetConnectGTS();
//TODO
void CMD_15F();
//TODO
void NetConnectWiFiBattle(short p0, short p1);
//TODO
void NetConnectBattleVideo(short p0);
//TODO
void NetConnectGTSNegotiation();
//TODO
void CMD_163(short p0, char p1);
//TODO
void CMD_164(short p0);
//TODO
void CMD_165(char p0, short p1, short p2);
//TODO
void CMD_166(short p0, char p1, short p2);
//TODO
void CMD_167(short p0, char p1);
//TODO
void CMD_168(short p0);
//TODO
void CMD_169(char p0, short p1, short p2);
//TODO
void CMD_16a(short p0, char p1, short p2);
//TODO
void CMD_16b(short p0, short p1, short p2, short p3);
//TODO
void CMD_16c(short p0);
//TODO
void CMD_16d(short p0);
//TODO
void CMD_16e(short p0, short p1);
//TODO
void CMD_16f();
//TODO
void CMD_170(short p0);
//TODO
void CMD_171();
//TODO
void CMD_172(short p0);
//TODO
void CMD_173(short p0);
//TODO
void CMD_174(short p0, short p1, short p2);
//TODO
void CMD_175(short p0);
//TODO
void CMD_176(short p0);
//TODO
void CMD_177();
//TODO
void CallWildBattle(short species, short level, short flags);
//TODO
void CallWildBattleEnd();
//TODO
void CallWildLose();
//TODO
void WildBattleIsVictory(short dest);
//TODO
void WildBattleGetResult(short dest);
//TODO
void CMD_17d(short p0);
//TODO
void CMD_17e(short p0);
//TODO
void CMD_17f(short p0, short p1);
//TODO
void CMD_180(short p0);
//TODO
void CMD_181(short p0);
//TODO
void CMD_182(short p0);
//TODO
void CMD_183();
//TODO
void CMD_184(short p0);
//TODO
void CMD_185(short p0);
//TODO
void CMD_186(short p0);
//TODO
void CMD_187(short p0);
//TODO
void CMD_188(short p0);
//TODO
void CMD_189(short p0);
//TODO
void CMD_18a();
//TODO
void CMD_18b();
//TODO
void CMD_18c(short p0, short p1);
//TODO
void CMD_18d(short p0);
//TODO
void CMD_18e(short p0);
//TODO
void CMD_18f(short p0, short p1, short p2, short p3, short p4, short p5, short p6, short p7);
//TODO
void CMD_190(short p0);
//TODO
void CMD_191(short p0);
//TODO
void CMD_192(short p0);
//TODO
void CMD_193(short p0);
//TODO
void CMD_194(short p0);
//TODO
void CMD_195(short p0);
//TODO
void CMD_196();
//TODO
void CMD_197(short p0);
//TODO
void CMD_198(short p0);
//TODO
void CMD_199(short p0);
//TODO
void CMD_19a(short p0, short p1);
//TODO
void CMD_19b(short p0);
//TODO
void CMD_19c(short p0);
//TODO
void CMD_19d(short p0);
//TODO
void CMD_19e(short p0);
//TODO
void PlayFieldEffect(short effectNo);
//TODO
void PlayHMCutInEffect(short partyIdx);
//TODO
void PlayAlderFlyEffect(short actorId);
//TODO
void CMD_1a2(short p0);
//TODO
void CMD_1a3(short p0);
//TODO
void CMD_1a4(short p0);
//TODO
void CMD_1a5();
//TODO
void CMD_1a6();
//TODO
void CMD_1a7(short p0, short p1);
//TODO
void CMD_1a8();
//TODO
void CMD_1a9(short p0, short p1, short p2, short p3);
//TODO
void CMD_1aa(short p0, short p1, short p2, short p3);
//TODO
void CMD_1ab(short p0);
//TODO
void CMD_1ac();
//TODO
void CMD_1ad();
//TODO
void CMD_1ae();
//TODO
void CMD_1af();
//TODO
void CMD_1b0(short p0, short p1);
//TODO
void CMD_1b1();
//TODO
void CMD_1b2(short p0);
//TODO
void CMD_1b3(short p0, short p1);
//TODO
void CMD_1b4(short p0, short p1);
//TODO
void CMD_1b5();
//TODO
void CMD_1b6();
//TODO
void CMD_1b7();
//TODO
void CMD_1b8();
//TODO
void CMD_1b9(short p0, short p1);
//TODO
void CMD_1ba(short p0, short p1);
//TODO
void CMD_1bb();
//TODO
void CMD_1bc(short p0);
//TODO
void CMD_1bd(short p0, short p1);
//TODO
void FieldTradeStart(short tradeNo, short partyIdx);
//TODO
void FieldTradeCheck(short dest, short tradeNo, short partyIdx);
//TODO
void ElevatorSetTablePtr(int offset);
//TODO
void ElevatorBuildListMenu();
//TODO
void ElevatorChangeMap(short floorIdx);
//TODO
void PokeDexIsComplete(short dest, short mode);
//TODO
void PokeDexGetEvaluationParams(short mode, short destMsgId, short destCaughtNo, short destMEId);
//TODO
void PokeDexGiveNational();
//TODO
void PokeDexHaveNational(short dest);
//TODO
void PokeDexEnable();
//TODO
void CMD_1c9(short p0, short p1);
//TODO
void CMD_1ca();
//TODO
void CMD_1cb(int p0);
//TODO
void CMD_1cc();
//TODO
void CMD_1cd(short p0);
//TODO
void CMD_1ce(short p0, short p1);
//TODO
void CMD_1cf(short p0, short p1, short p2);
//TODO
void CMD_1d0(short p0, short p1, short p2, short p3);
//TODO
void CMD_1d1(short p0);
//TODO
void CMD_1d2(short p0, short p1, short p2);
//TODO
void CMD_1d3(short p0, short p1, short p2);
//TODO
void CMD_1d4();
//TODO
void CMD_1d5();
//TODO
void CMD_1d6(short p0);
//TODO
void ObjInitProxyGPos(short proxyIndex, short x, short y, short z);
//TODO
void ObjInitWarpGPos(short warpIndex, short x, short y, short z);
//TODO
void ObjInitNPCGPos(short npcIndex, short dir, short x, short y, short z);
//TODO
void CallPhraseSelect(short questionId, short pAnswerId, short pSuccess, short p3);
//TODO
void CMD_1db(short p0);
//TODO
void CMD_1dc(short p0, short p1);
//TODO
void CMD_1dd(short p0, short p1);
//TODO
void CMD_1de(short p0, short p1);
//TODO
void CMD_1df(short p0, short p1, short p2);
//TODO
void CMD_1e0(short p0, short p1, short p2, short p3);
//TODO
void CMD_1e1(short p0);
//TODO
void CMD_1e2();
//TODO
void CMD_1e3(short p0);
//TODO
void CMD_1e4(short p0, short p1, short p2, short p3, short p4, short p5);
//TODO
void CMD_1e5();
//TODO
void CMD_1e6();
//TODO
void CMD_1e7();
//TODO
void CMD_1e8(short p0, short p1);
//TODO
void CMD_1e9(short p0, short p1);
//TODO
void CMD_1ea(short p0, short p1, short p2, short p3);
//TODO
void CMD_1eb(short p0, short p1, short p2, short p3);
//TODO
void CMD_1ec(short p0, short p1, short p2, short p3, short p4);
//TODO
void CMD_1ed(short p0, short p1, short p2, short p3);
//TODO
void CMD_1ee(short p0, short p1);
//TODO
void CMD_1ef(short p0, short p1);
//TODO
void CMD_1f0(short p0, short p1);
//TODO
void CMD_1f1(short p0, short p1);
//TODO
void CMD_1f2();
//TODO
void CMD_1f3(short p0);
//TODO
void CMD_1f4();
//TODO
void CMD_1f5();
//TODO
void CMD_1f6(short p0, short p1, short p2);
//TODO
void CMD_1f7(short p0, short p1, short p2, short p3, short p4, short p5);
//TODO
void CMD_1f8();
//TODO
void CMD_1f9();
//TODO
void CMD_1fa();
//TODO
void CMD_1fb(short p0, short p1);
//TODO
void CMD_1fc(short p0, short p1, short p2);
//TODO
void CMD_1fd(short p0, short p1, short p2, short p3);
//TODO
void CMD_1fe();
//TODO
void CMD_1ff(short p0);
//TODO
void CMD_200(short p0, short p1, short p2);
//TODO
void CMD_201(short p0, short p1);
//TODO
void CMD_202(short p0);
//TODO
void CMD_203(short p0);
//TODO
void CMD_204(short p0);
//TODO
void CMD_205(short p0);
//TODO
void CMD_206(short p0);
//TODO
void CMD_207(short p0, short p1);
//TODO
void CMD_208(short p0);
//TODO
void CMD_209(short p0, short p1, short p2, short p3);
//TODO
void CMD_20a(short p0, short p1, short p2, short p3);
//TODO
void CMD_20b(short p0);
//TODO
void CMD_20c(short p0, short p1, short p2, short p3);
//TODO
void CMD_20d(short p0, short p1, short p2, short p3);
//TODO
void CMD_20e(short p0, short p1, short p2, short p3, short p4, short p5);
//TODO
void CMD_20f(short p0, short p1, short p2);
//TODO
void CMD_210(short p0);
//TODO
void CMD_211(short p0);
//TODO
void CMD_212(short p0, short p1, short p2);
//TODO
void CMD_213(short p0);
//TODO
void CMD_214(short p0, short p1, short p2, short p3);
//TODO
void CMD_215(short p0, short p1);
//TODO
void CMD_216(short p0, short p1);
//TODO
void CMD_217(short p0);
//TODO
void CMD_218(short p0, short p1);
//TODO
void CMD_219(short p0, short p1);
//TODO
void CMD_21a(short p0, short p1);
//TODO
void CMD_21b();
//TODO
void CMD_21c(short p0, short p1);
//TODO
void CMD_21d(short p0);
//TODO
void CMD_21e(short p0);
//TODO
void CMD_21f();
//TODO
void PokePartyFindEx(short species, short forme, short pPartyIdx, short pSuccess);
//TODO
void CMD_221(short p0, short p1, short p2, short p3, short p4, short p5);
//TODO
void CMD_222(short p0, short p1, short p2, short p3);
//TODO
void CMD_223(short p0);
//TODO
void CMD_224(short p0);
//TODO
void CMD_225(short p0, short p1, short p2);
//TODO
void CMD_226(short p0);
//TODO
void CMD_227(short p0, short p1, short p2, short p3);
//TODO
void CMD_228(short p0, short p1);
//TODO
void CMD_229(short p0, short p1);
//TODO
void CMD_22a(short p0);
//TODO
void CMD_22b(short p0, short p1);
//TODO
void CMD_22d(short p0);
//TODO
void CMD_22e(short p0);
//TODO
void CMD_22f(short p0, short p1);
//TODO
void CMD_230(short p0, short p1);
//TODO
void CMD_231(short p0);
//TODO
void CMD_232(short p0, short p1);
//TODO
void CMD_233(short p0, short p1);
//TODO
void CMD_234(short p0, short p1);
//TODO
void CMD_235(short p0, short p1);
//TODO
void CMD_236(short p0, short p1, short p2, short p3);
//TODO
void CMD_237(short p0, short p1);
//TODO
void CMD_238(short p0);
//TODO
void CMD_239(short p0);
//TODO
void CMD_23a(short p0, short p1);
//TODO
void CMD_23b(short p0, short p1);
//TODO
void CMD_23c();
//TODO
void CMD_23d(short p0, short p1);
//TODO
void CMD_23e(short p0, short p1);
//TODO
void CMD_23f();
//TODO
void CMD_240(short p0);
//TODO
void CMD_241(short p0);
//TODO
void CMD_242(short p0, short p1);
//TODO
void CMD_243(short p0, short p1);
//TODO
void CMD_244(short p0);
//TODO
void CMD_245(short p0, short p1);
//TODO
void CMD_246(short p0);
//TODO
void CMD_247(short p0, short p1, short p2, short p3, short p4);
//TODO
void CMD_248(short p0, short p1);
//TODO
void CMD_249(short p0, short p1, short p2, short p3);
//TODO
void CMD_24a(short p0);
//TODO
void CMD_24b(short p0);
//TODO
void CMD_24c(short p0);
//TODO
void CMD_24d();
//TODO
void CMD_24e(short p0, short p1);
//TODO
void CMD_24f(short p0, short p1, short p2, short p3, short p4, short p5);
//TODO
void CMD_250(short p0, short p1, short p2, short p3, short p4);
//TODO
void CMD_251(short p0, short p1, short p2);
//TODO
void CMD_252();
//TODO
void CMD_253(short p0, short p1);
//TODO
void CMD_254(short p0);
//TODO
void CMD_255(short p0, short p1, short p2, short p3, short p4, short p5);
//TODO
void CMD_256(short p0, short p1, short p2);
//TODO
void CMD_257();
//TODO
void CMD_258();
//TODO
void CMD_259(short p0);
//TODO
void CMD_25a(short p0);
//TODO
void CMD_25b();
//TODO
void CMD_25c(short p0, short p1, short p2, short p3, short p4, short p5);
//TODO
void CMD_25d(short p0);
//TODO
void CMD_25e();
//TODO
void CMD_25f();
//TODO
void CMD_260();