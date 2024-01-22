export abstract class DeviceServiceBase {
    public abstract get isSSR(): boolean;
    public abstract get devicePixelRatio(): number;
}