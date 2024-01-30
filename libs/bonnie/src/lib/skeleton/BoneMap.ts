import { Type } from "@angular/core";
import { IBoneComponent } from "./IBoneComponent";


export class BoneMap extends Map<string, Type<IBoneComponent>>
{
  public getRequired(key: string): Type<IBoneComponent> {
    const value = this.get(key);

    if (value === undefined || value === null) {
      throw Error(`BoneMap doesn't contain type for key = ${key}`);
    }

    return value;
  }
}
