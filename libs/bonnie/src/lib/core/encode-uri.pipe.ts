import { Pipe, PipeTransform } from "@angular/core";

@Pipe({ name: "encodeURIComponent", standalone: true, pure: true })
export class EncodeURIComponentPipe implements PipeTransform {
	public transform(value: string): string {
		return encodeURIComponent(value);
	}
}
