import { Type } from '@angular/core';
import { IBoneEditor } from './IBoneEditor';

export class BoneEditorMap extends Map<string, Type<IBoneEditor>>
{
  public getRequired(key: string): Type<IBoneEditor> {
    const value = this.get(key);

    if (value === undefined || value === null) {
      throw Error(`BoneMap doesn't contain type for key = ${key}`);
    }

    return value;
  }
}
