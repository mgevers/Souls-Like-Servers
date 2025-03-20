export type Envelopes = MonsterEnvelopes

export type MonsterEnvelopes = MonsterSuccessEnvelopes | MonsterFailureEnvelopes

export type MonsterSuccessEnvelopes = MonsterAddedEnvelope
  | MonsterRemovedEnvelope
  | MonsterUpdatedEnvelope
  | MonsterAttributeSetUpdatedEnvelope
  | MonsterNameUpdatedEnvelope
  | MonsterLevelUpdatedEnvelope

export type MonsterFailureEnvelopes = FailedToAddMonsterEnvelope
  | FailedToRemoveMonsterEnvelope
  | FailedToUpdateMonsterEnvelope
  | FailedToUpdateMonsterAttributeSetEnvelope
  | FailedToUpdateMonsterLevelEnvelope
  | FailedToUpdateMonsterNameEnvelope

export function IsMonsterEnvelope(envelope: Envelopes): boolean {
  return IsMonsterSuccessEnvelope(envelope) || IsMonsterFailureEnvelope(envelope);
}

export function IsMonsterSuccessEnvelope(envelope: Envelopes): boolean {
  return envelope.eventType === 'MonsterAddedEvent'
    || envelope.eventType === 'MonsterRemovedEvent'
    || envelope.eventType === 'MonsterUpdatedEvent'
    || envelope.eventType === 'MonsterAttributeSetUpdatedEvent'
    || envelope.eventType === 'MonsterNameUpdatedEvent'
    || envelope.eventType === 'MonsterLevelUpdatedEvent'
}

export function IsMonsterFailureEnvelope(envelope: Envelopes): boolean  {
  return envelope.eventType === 'FailedToAddMonsterEvent'
    || envelope.eventType === 'FailedToRemoveMonsterEvent'
    || envelope.eventType === 'FailedToUpdateMonsterEvent'
    || envelope.eventType === 'FailedToUpdateMonsterAttributeSetEvent'
    || envelope.eventType === 'FailedToUpdateMonsterNameEvent'
    || envelope.eventType === 'FailedToUpdateMonsterLevelEvent'
}

export enum ResultStatus {
  Ok = 0,
  Created = 1,
  Error = 2,
  Forbidden = 3,
  Unauthorized = 4,
  Invalid = 5,
  NotFound = 6,
  NoContent = 7,
  Conflict = 8,
  CriticalError = 9,
  Unavailable = 10,
}

export type FailureEvent = {
  status: ResultStatus,
  errors: string[],
}

export type MonsterAddedEnvelope = {
  eventType: 'MonsterAddedEvent',
  payload: MonsterAddedEvent
}

export type MonsterRemovedEnvelope = {
  eventType: 'MonsterRemovedEvent',
  payload: MonsterRemovedEvent
}

export type MonsterUpdatedEnvelope = {
  eventType: 'MonsterUpdatedEvent',
  payload: MonsterUpdatedEvent
}

export type MonsterAttributeSetUpdatedEnvelope = {
  eventType: 'MonsterAttributeSetUpdatedEvent',
  payload: MonsterAttributeSetUpdatedEvent
}

export type MonsterNameUpdatedEnvelope = {
  eventType: 'MonsterNameUpdatedEvent',
  payload: MonsterNameUpdatedEvent
}

export type MonsterLevelUpdatedEnvelope = {
  eventType: 'MonsterLevelUpdatedEvent',
  payload: MonsterLevelUpdatedEvent
}

export type FailedToAddMonsterEnvelope = {
  eventType: 'FailedToAddMonsterEvent',
  payload: FailedToAddMonsterEvent
}

export type FailedToRemoveMonsterEnvelope = {
  eventType: 'FailedToRemoveMonsterEvent',
  payload: FailedToRemoveMonsterEvent
}

export type FailedToUpdateMonsterEnvelope = {
  eventType: 'FailedToUpdateMonsterEvent',
  payload: FailedToUpdateMonsterEvent
}

export type FailedToUpdateMonsterAttributeSetEnvelope = {
  eventType: 'FailedToUpdateMonsterAttributeSetEvent',
  payload: FailedToUpdateMonsterAttributeSetEvent
}

export type FailedToUpdateMonsterNameEnvelope = {
  eventType: 'FailedToUpdateMonsterNameEvent',
  payload: FailedToUpdateMonsterNameEvent
}

export type FailedToUpdateMonsterLevelEnvelope = {
  eventType: 'FailedToUpdateMonsterLevelEvent',
  payload: FailedToUpdateMonsterLevelEvent
}

export type MonsterAddedEvent = {
  monsterId: string,
  monsterName: MonsterName,
  monsterLevel: MonsterLevel,
  attributeSet: SoulsAttributeSet,
}

export type MonsterRemovedEvent = {
  monsterId: string,
}

export type MonsterUpdatedEvent = {
  monsterId: string,
  monsterName: MonsterName,
  monsterLevel: MonsterLevel,
  attributeSet: SoulsAttributeSet,
}

export type MonsterAttributeSetUpdatedEvent = {
  monsterId: string,
  attributeSet: SoulsAttributeSet,
}

export type MonsterNameUpdatedEvent = {
  monsterId: string,
  monsterName: MonsterName,
}

export type MonsterLevelUpdatedEvent = {
  monsterId: string,
  monsterLevel: MonsterLevel,
}

export type FailedToAddMonsterEvent = FailureEvent & {
  monsterId: string,
}

export type FailedToRemoveMonsterEvent = FailureEvent & {
  monsterId: string,
}

export type FailedToUpdateMonsterEvent = FailureEvent & {
  monsterId: string,
  monsterName: MonsterName,
  monsterLevel: MonsterLevel,
  attributeSet: SoulsAttributeSet,
}

export type FailedToUpdateMonsterAttributeSetEvent = FailureEvent & {
  monsterId: string,
  attributeSet: SoulsAttributeSet,
}

export type FailedToUpdateMonsterNameEvent = FailureEvent & {
  monsterId: string,
  monsterName: MonsterName,
}

export type FailedToUpdateMonsterLevelEvent = FailureEvent & {
  monsterId: string,
  monsterLevel: MonsterLevel,
}

export type MonsterName = {
  value: string,
}

export type MonsterLevel = {
  value: number,
}

export type SoulsAttributeName = 'maxHealth'
  | 'maxMana'
  | 'maxStamina'
  | 'physicalPower'
  | 'physicalDefense' 

export type SoulsAttributeSet = {
  maxHealth?: number;
  maxMana?: number;
  maxStamina?: number;
  physicalPower?: number;
  physicalDefense?: number;
}
  
export type Monster = {
  id: string,
  monsterName: string;
  monsterLevel: number;
  attributeSet: SoulsAttributeSet;
}
  
export type Item = {
  id: string,
  itemName: string;
  attributeSet: SoulsAttributeSet;
}
  
export type AddMonsterRequest = {
  monsterId: string;
  monsterName: string;
  monsterLevel: number;
  attributeSet: SoulsAttributeSet;
}
  
export type UpdateMonsterRequest = {
  monsterId: string;
  monsterName: string;
  monsterLevel: number;
  attributeSet: SoulsAttributeSet;
}
