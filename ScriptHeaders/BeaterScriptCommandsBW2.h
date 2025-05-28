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
//Finished all script sub-events that certain commands may have invoked.
void RTFinishSubEvents();
//Waits until "A" or "B" is pressed.
void InputWaitAB();
//Waits until any of A/B/Up/Down/Left/Right is pressed.
void InputWaitLast();
//Enables/disabled message box auto-scrolling.
void MsgSetAutoscrolls(short enabled);
//Opens a system message box.
void MsgSystem(short msgId, short pos);
//Opens a system message box, but allows additional code to run while it's up.
void MsgSystemAsync(short msgId, short pos);
//Closes the system message box.
void MsgSystemClose();
//Shows/hides a loading spinner at the system message box.
void MsgSetLoadingSpinner(short hide);
//Opens a message which isn't associated with an NPC.
void MsgInfo(short msgId, char pos);
//Closes the message which isn't associated with an NPC.
void MsgInfoClose();
//Opens a message at position (x, y). The coordinates are based on screen hardware tiles (8 pixels).
void MsgMulti(short msgId, short x, short y, short handle);
//Closes a message window handle.
void MsgWinCloseNo(short handle);
//Open a message window bound to an actor. The actor is specified manually.
void MsgActorEx(short textFile, short msgId, short actorId, short pos, short type);
//Open a message window bound to an actor. The actor is automatically determined.
void MsgActor(short textFile, short msgId, short pos, short type);
//Closes the actor message.
void MsgActorClose();
//Closes all message windows.
void MsgWinCloseAll();
//Shows the money box at coords (x, y).
void MoneyWinDisp(short x, short y);
//Closes the money box.
void MoneyWinClose();
//Updates the values in the money box.
void MoneyWinUpdate();
//Opens a place sign message window.
void MsgPlaceSign(short msgId, short type);
//Closes the place sign message window.
void MsgPlaceSignClose();
//Opens a message with a checkerboard background.
void MsgCheckerBG(short msgId, char posX, char posY, short unused);
//Closes the checkerboard message.
void MsgCheckerBGClose();
//Opens a box with two options, yes or no. The result of the choice made by the player is stored in dest.
void YesNoWin(short dest);
//A message which changes depending on the player's gender.
void MsgActorGendered(short textFile, short msgIdM, short msgIdF, short actorId, short pos, short type);
//A message which changes depending on whether the game is Black or White.
void MsgActorVersioned(short textFile, short msgIdW, short msgIdB, short actorId, short pos, short type);
//Opens a message with a spiked border. Known as the angry message by some.
void MsgScream(short id, char pos);
//Waits until all messages have finished scrolling.
void MsgWaitAdvance();
//Stores the name of the player to a string buffer.
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
//Stores an Actor's XZ grid location to two variables.
void ActorGetGPos(short actorId, short pX, short pZ);
//Stores the player's XZ grid location to two variables.
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
//Performs an animated player sequence such as item pickup.
void PlayerSetSpecialSequence(short seqId);
//Moves the player to a given Y location asynchronously.
void PlayerMoveToYAsync(short direction, short interval, short destY, short isYPositive);
//If the player is currently standing in the area of a trigger with a direction value set, they will turn in that direction.
void PlayerTurnByTrigger();
//walk/surf/dive
void PlayerGetExState(short dest);
//TODO
void ActorGetUserParam(short actorId, short paramIdx, short dest);
//Overlay 131 - rail_slipdown.c
void ActorPlayRailSlipdown(short actorId);
//Makes an Actor jump to specified XYZ coordinates on map (in that order).
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
//TODO - Flags: 1 - Continue after lose
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
//Plays a trainer eye clash BGM by a trainer class ID.
void TrainerClassBGMPlayPush(short classId);
//Plays a trainer eye clash BGM by a trainer ID.
void TrainerBGMPlay(short trId);
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
//The partyIdx parameter is unused.
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
//The direction passed to this method should be incremented by one because of a game bug.
void FieldSetNextZone(short zoneId, short dir, short x, short y, short z);
//TODO
void PokeDexGetCount(short caughtOnly, short dest);
//Mode can only be 0.
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
void CMD_E7(short p0);
//TODO
void CMD_E8(short p0, short p1);
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
void CallPokeSelect(short wantEgg, short pDlgSuccess, short pPartyIdx, short unused);
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
void FieldSetWeather(short type, short p1);
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
void CMD_13C();
//TODO
void CMD_13D();
//TODO
void CMD_13E();
//Initializes the event camera. This stores the current coordinates for use with Return. If this function is not called, the Return function will be broken and the camera will not be able to return to the default values.
void EVCameraInit();
//Releases the event camera.
void EVCameraEnd();
//Unbinds the event camera from the player character.
void EVCameraUnbind();
//Binds the event camera back to the player character, if the configuration allows it.
void EVCameraRebind();
//TODO
void EVCameraMoveTo(short pitch, short yaw, int distance, int targetX, int targetY, int targetZ, short interval);
//Resets the camera to coordinates saved when EVCamera.Init was last called.
void EVCameraReturn(short interval);
//TODO
void EVCameraWait();
//TODO
void EVCameraMoveToCommon(short id, short interval);
//Resets the camera to data read from the default camera NARC.
void EVCameraMoveToDefault(short interval);
//TODO
void EVCameraShake(short intensityH, short intensityW, short loopLength, short loopCount, short decayX, short decayY, short decayStartLoop, short decayStepLength);
//TODO
void CallFriendlyShopBuy(short shopId, short unused);
//Closes the Field module to free up resources for standalone events such as 3D cutscenes.
void FieldOpen();
//Re-opens the Field module.
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
//Calls the starter selection and stores the result to a work value.
void CallPoke3Select(short dest);
//Plays a 3D cutscene based on its ID and a user parameter for special cutscenes (ferris wheel).
void Call3DDemo(short seqId, short userParam);
//TODO
void CallXTransceiver(short sceneId, short p1);
//Plays the Hall of Fame induction scene and the staff roll. The "repeated" param declares if the game should act like the HoF has already been entered before or not.
void CallGameClear(short repeated);
//TODO
void ActorAnimationInit(short actorId);
//TODO
void ActorAnimationFree();
//anmId is index into a/2/0/0.
void ActorAnimationPlay(short anmId);
//TODO
void ActorAnimationWait();
//TODO
void CMD_15B(short p0);
//TODO
void CMD_15C(short p0);
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
void CMD_167(short p0, short p1, short p2, short p3);
//TODO
void CMD_168(short p0);
//TODO
void CMD_169(short p0);
//TODO
void CMD_16A(short p0, short p1);
//TODO
void CMD_16E();
//TODO
void CMD_170(short p0);
//TODO
void CMD_172(short p0);
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
//Calls the game-specific Pokémon catching tutorial.
void CallCaptureDemo();
//Used for gatehouse electric board gimmick.
void CMD_17A(short p0);
//TODO
void CMD_17B(short p0, short p1);
//TODO
void CMD_17C(short p0);
//TODO
void CMD_17D(short p0);
//TODO
void CMD_186();
//TODO
void CMD_187(short p0);
//TODO
void CMD_18C(short p0, short p1);
//TODO
void CMD_18D(short p0);
//TODO
void CMD_18E(short p0);
//TODO
void CMD_18F(short p0);
//TODO
void CMD_190(short p0);
//TODO
void CMD_191(short p0);
//TODO
void CGearPowerOn(short enableComms);
//subscreenId is a LUT index, where 0 = subscreen 7 (1 entry)
void FieldSubscreenChange(short subscreenId);
//TODO
void FieldSubscreenReturn();
//TODO
void CGearControlWarning(short isVisible);
//Plays a general field effect. F.e. the Kyurem blizzard.
void PlayFieldEffect(short effectNo);
//TODO
void PlayHMCutInEffect(short partyIdx);
//TODO
void PlayAlderFlyEffect(short actorId);
//TODO
void CMD_1A1(short p0, short p1, short p2, short p3);
//TODO
void CallRoyalUnovaView(short pIsCruiseOver);
//TODO
void FadeInBlackQ();
//TODO
void FadeOutBlackQ();
//TODO
void FadeInWhiteQ();
//TODO
void FadeOutWhiteQ();
//TODO
void FadeWait();
//TODO
void FadeInBlackQ_();
//TODO
void CallSeasonBanner();
//Makes both screens black and fades out from it.
void FadeInBlack();
//Fades both screens into black.
void FadeOutBlack();
//Makes both screens white and fades out from it.
void FadeInWhite();
//Fades both screens into white.
void FadeOutWhite();
//TODO
void CMD_1AF(short p0, short p1);
//TODO
void CMD_1B0(short p0, short p1);
//TODO
void CallLeagueLiftWarp();
//TODO
void CMD_1B2(short p0);
//TODO
void CMD_1B3(short p0, short p1);
//TradeNo = index into a/1/6/3.
void FieldTradeStart(short tradeNo, short partyIdx);
//TODO
void FieldTradeCheck(short dest, short tradeNo, short partyIdx);
//TODO
void ElevatorSetTablePtr(int offset);
//TODO
void ElevatorBuildListMenu();
//TODO
void ElevatorChangeMap(short floorIdx);
//mode=national/seen unova/caught unova
void PokeDexIsComplete(short dest, short mode);
//mode same as above
void PokeDexGetEvaluationParams(short mode, short destMsgId, short destCaughtNo, short destMEId);
//TODO
void PokeDexGiveNational();
//TODO
void PokeDexHaveNational(short dest);
//TODO
void PokeDexEnable();
//TODO
void NDemoStart(short backgroundId, short unusedArg, short fadeBothScreens);
//TODO
void NDemoEnd();
//TODO
void NDemoReadyTalkMotion();
//TODO
void CMD_1CC(short p0);
//TODO
void CMD_1CD(short p0);
//TODO
void CMD_1CE(short p0, short p1);
//paramId = guest count/floor country/floor number
void UnityTowerGetStateParam(short paramId, short dest);
//TODO
void UnityTowerCallFloorSelect(short initialFloor, short pFloor, short pCountry, short pSuccess);
//tutor type = draco meteor/ultimate moves/pledge moves
void MoveTutorCheckParty(short tutorType, short dest);
//TODO
void MoveTutorCheckPkm(short tutorType, short partyIdx, short dest);
//TODO
void MoveTutorGetMoveID(short tutorType, short partyIdx, short dest);
//TODO
void MoveTutorCallPokeSelect(short tutorType, short pSuccess, short pPartyIdx);
//TODO
void MoveReminderCheckPkm(short dest, short partyIdx);
//TODO
void MoveReminderCallMoveSelect(short pSuccess, short partyIdx);
//Sets the location of a Proxy zone entity.
void ObjInitProxyGPos(short proxyIndex, short x, short y, short z);
//Sets the location of a Warp zone entity.
void ObjInitWarpGPos(short warpIndex, short x, short y, short z);
//Sets the location and direction of an NPC zone entity. Note that this function only works in initialization scripts before Actors are spawned.
void ObjInitNPCGPos(short npcIndex, short dir, short x, short y, short z);
//TODO
void CallPhraseSelect(short questionId, short pAnswerId, short pSuccess, short p3);
//TODO
void CMD_1DB(short p0, short p1);
//TODO
void CMD_1DC(short p0, short p1);
//TODO
void CMD_1DD(short p0, short p1, short p2);
//TODO
void CMD_1DE(short p0, short p1);
//TODO
void CMD_1DF();
//TODO
void StadiumSetupActorsDouble(short group, short actorId1, short actorId2, short baseTrId);
//TODO
void StadiumLoadTrainerTable();
//TODO
void StadiumFreeTrainerTable();
//TODO
void StadiumSetupActorSingle(short group, short actorId1, short trId);
//TODO
void StadiumSetupActorsTriple(short actorId1, short actorId2, short actorId3, short trId1, short trId2, short trId3);
//TODO
void StadiumResetTrainerFlags();
//TODO
void TrialHouseWorkInit();
//TODO
void TrialHouseWorkDelete();
//single/double
void TrialHousePrepareParty(short battleType);
//single=20/double=21
void TrialHouseCallTeamSelect(short battleType, short useBattleBox, short pSuccess);
//TODO
void CMD_1EA(short p0, short p1);
//TODO
void TrialHouseMsgDisp(short trIndex, short actorId);
//TODO
void TrialHouseStartBattle();
//TODO
void CMD_1ED(short p0);
//TODO
void CMD_1EE(short p0);
//TODO
void TrialHouseGetBattleTestRank(short dest);
//TODO
void CMD_1F0(short p0);
//TODO
void TrialHouseCalcPointsStars(short pPoints, short pStars);
//TODO
void CMD_1F2(short p0);
//TODO
void TrialHouseSaveData(short p0);
//TODO
void CMD_1F4(short p0, short p1);
//TODO
void CMD_1F5(short p0);
//TODO
void CMD_1F6(short p0, short p1, short p2, short p3);
//TODO
void CMD_1F7(short p0, short p1, short p2, short p3);
//TODO
void CMD_1F8(short p0, short p1);
//TODO
void CMD_1F9(short p0, short p1, short p2, short p3);
//TODO
void PepQuizGenerate(short allowNatDexQuestions, short pQuestionMsgId, short pHintMsgId, short pAnswerId);
//TODO
void ItemCollectorCheckGroup(short itemCollectorId, short dest);
//TODO
void ItemCollectorGetPrice(short itemId, short itemCollectorId, short dest);
//TODO
void CMD_1FD(short p0, short p1, short p2, short p3);
//TODO
void CMD_1FE();
//TODO
void SurveyGetCurrentQuestionID(short dest);
//TODO
void SurveyGetCurrentAnswerIDs(short dest1, short dest2, short dest3);
//TODO
void SurveyGetPopularOptionMsgID(short surveyId, short dest);
//TODO
void CMD_202(short p0, short p1);
//TODO
void CMD_203(short p0);
//TODO
void SurveyGetTime(short pHours);
//TODO
void CallGreetingPhraseInput(short pSuccess);
//TODO
void CallThanksPhraseInput(short pSuccess);
//TODO
void CMD_207(char p0);
//TODO
void CMD_208(char p0);
//TODO
void CMD_209(short p0, short p1);
//TODO
void CMD_20A();
//TODO
void CMD_20B(short p0);
//TODO
void CMD_20C();
//TODO
void PokePartyFindEx(short species, short forme, short pPartyIdx, short pSuccess);
//Plays an animation of tossing a Poké Ball to specified co-ordinates. Needs the gimmick from places like Pledge Grove or Café Sonata in Castelia City to work.
void CMD_20E(short isReturning, short targetX, short targetY, short targetZ, short arcYMaxPos, short speed);
//Plays an animation of opening a Poké Ball at specified co-ordinates. Needs the gimmick from places like Pledge Grove or Café Sonata in Castelia City to work.
void CMD_20F(short isReturning, short targetX, short targetY, short targetZ);
//TODO
void CMD_210(short isReturning);
//TODO
void CMD_211(short isReturning);
//TODO
void CMD_212(short p0, short p1, short p2);
//TODO
void CMD_213(short p0);
//TODO
void CallPokemonPreview(short species, short forme, short sex, short shiny);
//TODO
void CMD_215(short p0, short p1);
//TODO
void CMD_216(short p0, short p1);
//TODO
void MapChangeEntreeForest(short moveDirection);
//TODO
void CMD_218(short p0, short p1);
//TODO
void CMD_219(short p0, short p1);
//TODO
void WordSetEntreeForestPkmName(short actorId, short strbufIdx);
//TODO
void EntreeForestSpawnAllPkm();
//TODO
void EntreeForestStartBattle(short actorId, short pResult);
//TODO
void CMD_21D(short p0);
//TODO
void FishingChallengeGetRandomPkm(short dest);
//TODO
void CMD_21F(short p0, short p1);
//TODO
void CMD_220(short p0, short p1);
//TODO
void CMD_221(short p0, short p1);
//TODO
void CMD_222(short p0);
//TODO
void PokePartyGetHiddenPowerType(short dest, short partyIdx);
//statNo = sum/max/hp/atk/def/spe/spa/spd (0 - 7)
void PokePartyGetIV(short partyIdx, short paramId, short dest);
//TODO
void ItemGetRandomOwnedTMMove(short dest);
//TODO
void CMD_226(short p0);
//TODO
void RecordAdd(short recordId, short amount);
//TODO
void RecordGet(short recordId, short dest);
//TODO
void CMD_22A(short p0, short p1);
//TODO
void CMD_22B(short p0, short p1);
//TODO
void CMD_22D(short p0);
//TODO
void CMD_22E(short p0);
//TODO
void WordSetPastTradePkmName(short savePkmIdx, short strbufIdx);
//TODO
void CMD_230(short p0, short p1);
//TODO
void CMD_231(short p0);
//TODO
void CMD_232(short p0, short p1);
//TODO
void CMD_233(short p0);
//TODO
void ActorFallDownToXZ(short actorId, short x, short z, short dir);
//TODO
void BGMPlayEx(short sndId, short interval);
//TODO
void WordSetLoadItemCollectorPrice(short itemId, short itemCollectorId, short strbufIdx, short magnitude);
//TODO
void ItemCollectorSell(short itemId, short itemCollectorId);
//Interval = now bgm fade out time
void BGMChangeMapEx(short interval);
//TODO
void CMD_239(short p0);
//TODO
void CMD_23A(short p0, short p1);
//TODO
void CMD_23B();
//TODO
void CMD_23C();
//TODO
void CMD_23D();
//TODO
void DayCareGetSexForNamePrint(short dest, short showSexSign, short daycareSlot);
//TODO
void CallPlaceNameDisp();
//TODO
void CMD_240(short vol, short interval);
//TODO
void CMD_241(short interval);
//TODO
void BGMFadeOut(short interval);
//Fades ambience alongside BGM.
void BGMFadeOutAll(short interval);
//TODO
void MapChangeRail(short zoneId, short lineId, short posFront, short posSide, short dir);
//Direction = 0 -> source is procedural Y, dest is y. 1 -> source is y, dest is procedural Y.
void PlayerMoveToYAsync_(short direction, short interval, short destY, short isYPositive);
//TODO
void CMD_249(short p0, short p1, short p2, short p3, short p4, short p5);
//TODO
void CMD_24A(short p0);
//TODO
void FieldSubscreenDisable();
//TODO
void BGMAmbienceResume();
//TODO
void CMD_24D();
//TODO
void PokePartyIsFullPP(short dest, short partyIdx);
//Flags - 1=Ignore NPCs, 2=Ignore Collisions, 4=??? (Might be related to matrix patches?)
void ActorWalkRoute(short actorId, short destX, short destZ, short flags, short acmd, short pathfindingType);
//Actor ID is the unique ID of the field actor to attach. The Pair ID is a general identifier of the pair used in gameplay, it does not have any visible effects apart from setting work 16451, which can be used to turn off multi battles if set to 255 - not that this value, however, must not be higher than 7. If the "global" flag is set, the actor will persist when changing zones, BUT it will also not return to its original actor state when detached, so measures have to be taken to ensure the actor is properly deleted. The btlTrId is a trainer ID to use when in a multi battle, but most trainers will not work as they do not have a back sprite, so keep that in mind. ScrId can be 0 to keep current script ID and it will NOT be automatically restored on detach if changed.
void ActorPairSet(short actorId, short pairId, short isGlobal, short btlTrId, short scrId);
//moveCode and SCRID have to be manually defined because the game does not remember them from before the attachment.
void ActorPairEnd(short scrId, short moveCode);
//TODO
void ActorPairGetTrID(short dest);
//TODO
void ActorPairSetMoveEnable(char enable);
//TODO
void CMD_25A(short p0);
//TODO
void CMD_25B();
//TODO
void CMD_25C(short p0, short p1, short p2);
//TODO
void CMD_25D(short p0);
//TODO
void CMD_25F(short p0);
//TODO
void CMD_262(short p0, short p1);
//TODO
void CMD_263(short p0);
//TODO
void CMD_264(short p0, short p1);
//TODO
void CMD_265(short p0);
//TODO
void CMD_266(short p0);
//TODO
void CMD_267();
//TODO
void CMD_268(short p0);
//TODO
void CMD_269();
//TODO
void WordSetMedalName(char strbufIdx, short medalId);
//TODO
void WordSetMedalRank(char strbufIdx, short medalRankIdx);
//mode=hint medals/initial hint medals/pending medals/obtained medals/next rank medal requirement/hint medals remaining/medals remaining/medal rank
void MedalGetCount(char mode, short dest);
//mode=discover initial/acknowledge
void MedalAcknowledge(short medalId, short mode);
//TODO
void MedalGetGuruActor(short p0, short p1);
//TODO
void MedalGive(short medalId);
//TODO
void FunfestMissionStart();
//TODO
void CMD_275(char p0, short p1, short p2);
//TODO
void FunfestMissionBroadcast(short broadcastType, short value);
//TODO
void FunfestActorDelete();
//TODO
void FunfestDispSalesmanMessage(short baseMsgId, short stepPerPerson, short windowPos);
//TODO
void FunfestBGMReturn();
//TODO
void FunfestGetGenericInfo(short index, short dest);
//TODO
void FunfestGetItemExchangeInfo(short range1, short range2, short pWantedItem, short pOfferedItem);
//TODO
void FunfestGetItemSaleInfo(short pItemId, short pPrice);
//TODO
void FunfestGetPokemonQuizInfo(short pOptionCount, short pCorrectAnswer, short pQueriedSlotIdx);
//TODO
void FunfestGetPokemonQuizSpecies(short slotIndex, short dest);
//TODO
void FunfestGetPokemonQuizBogusSpecies(short slotIndex, short dest);
//TODO
void CallPlaceSelect(short pSuccess, short pZoneId);
//TODO
void CallWordSetPokeNameInput(short species, short strbufIdx, short pSuccess);
//TODO
void MapChangeFlyWarp(short zoneId, short direction);
//TODO
void CallEntralinkWarpOut();
//TODO
void CMD_28E(short p0);
//TODO
void CMD_28F(short p0);
//TODO
void WordSetRivalName(char strbufIdx);
//TODO
void CMD_291(short p0);
//TODO
void CMD_292(short p0);
//TODO
void HiddenHollowCallWarpIn(short hollowId);
//TODO
void HiddenHollowGetParam(short paramId, short dest);
//TODO
void HiddenHollowReset();
//TODO
void HiddenHollowCallWarpOut();
//TODO
void CallWildBattleEx(short species, short level, short forme, short flags);
//TODO
void CMD_298(char p0, short p1);
//TODO
void WordSetLoadAbility(char strbufIdx, short abil);
//TODO
void WordSetLoadNature(char strbufIdx, short nature);
//TODO
void WordSetLoadJoinAvenueName(char strbufIdx);
//TODO
void MedalGetFieldEffectID(short p0, short p1);
//TODO
void MedalDiscover(short medalId);
//TODO
void MedalIsObtained(short dest, short medalId);
//TODO
void MedalGetMostCompleteCategory(short dest);
//TODO
void CMD_2A2();
//TODO
void CMD_2A3();
//TODO
void CMD_2A4();
//TODO
void CMD_2A5(short p0);
//TODO
void CMD_2A6();
//TODO
void CMD_2A7(short p0);
//TODO
void CMD_2A8();
//TODO
void CMD_2A9();
//TODO
void PlayerPlayLeafPileStuck();
//TODO
void CMD_2AE(short p0);
//TODO
void GameGetDifficulty(short dest);
//TODO
void CallUnovaLinkKeyUnlock(short keyId);
//TODO
void CMD_2B1(short p0);
//TODO
void CMD_2B2(short p0, short p1);
//TODO
void CMD_2B3(short p0, short p1);
//TODO
void CMD_2B4(short p0, short p1);
//TODO
void CMD_2B5(short p0, short p1);
//TODO
void CMD_2B6(short p0, short p1);
//TODO
void CMD_2B7(short p0);
//TODO
void PedometerStart();
//TODO
void PedometerEnd();
//TODO
void PedometerGet(short dest);
//TODO
void Gym0601FanAmbienceStart();
//TODO
void TVCheckCommercial(short lowChance, short pPlayCommercial);
//TODO
void TVGenCommercialMsgID(short dest);
//TODO
void ActorMoveLinear(short actorId, short x, short y, short z, short interval);
//TODO
void FieldTradeGetSpecies(short dest, short tradeId);
//TODO
void RepelRearm(short pRepelType);
//TODO
void CMD_2C3(short p0);
//TODO
void CMD_2C4();
//TODO
void CMD_2C5(short p0);
//TODO
void JoinAvenueStoreStart();
//TODO
void JoinAvenueStoreEnd();
//TODO
void CallHappyPhraseInput(short pSuccess);
//TODO
void CMD_2CB(short p0);
//TODO
void PokeDexCheckHabitatList(short zoneId, short habitatId, short needsCaught, short dest);
//TODO
void PokeDexEnableHabitatList();
//TODO
void CMD_2D1(short p0);
//TODO
void FieldOpenRestoreLCD();
//TODO
void CMD_2D3(short p0, short p1);
//TODO
void ItemGetTMCount(short dest);
//TODO
void CMD_2D5(short p0, short p1);
//TODO
void CMD_2D7(short p0, short p1);
//TODO
void CMD_2D8(short p0);
//TODO
void CMD_2D9(short p0, short p1);
//TODO
void CMD_2DA(short p0);
//TODO
void UnityTowerSetFloor(short country, short floor);
//TODO
void UnityTowerInitVisitorMessage(short visitorHandleIdx, short tidFlavour, short pMsgId);
//TODO
void UnityTowerGetVisitorCount(short dest);
//TODO
void UnityTowerSetHobby(short hobbyId);
//TODO
void UnityTowerGetHobby(short dest);
//TODO
void MedalDiscoverInitial();
//TODO
void LensFlareRequest();
//TODO
void HiddenHollowSet(short hollowId, short group, short slot, short sex);
//TODO
void HiddenHollowCreateEvents();
//TODO
void CMD_2E8(short p0, short p1);
//TODO
void CMD_2E9(short p0, short p1);
//TODO
void PokePartyAddNPoke(short pSuccess, short species, short level, short nature, short abilChoice, short sex);
//TODO
void CMD_2ED(short p0, short p1);
//TODO
void MusicalIsPropOwned(short propId, short dest);
//TODO
void MusicalGetOwnedPropCount(short dest);
//TODO
void CMD_2F1(short p0);
//TODO
void CMD_2F2(short p0, short p1);