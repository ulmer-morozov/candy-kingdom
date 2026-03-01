import { Injectable } from "@angular/core";
import { DeviceServiceBase } from "./device.service.base";

@Injectable()
export class DeviceService implements DeviceServiceBase {
	public readonly devicePixelRatio = typeof window === "undefined" ? 1 : window.devicePixelRatio;
	public readonly isSSR = typeof window === "undefined";
}
