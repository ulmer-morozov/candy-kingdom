import { Pipe, type PipeTransform } from "@angular/core";

@Pipe({ name: "encodeURIComponent",  pure: true })
export class EncodeURIComponentPipe implements PipeTransform {
	public transform(value: string): string {
		return encodeURIComponent(value);
	}
}
