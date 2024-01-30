import {Component, ComponentFactoryResolver, EventEmitter, HostBinding, Input, OnChanges, Output, Type, ViewChild} from '@angular/core';
import {IBoneEditor} from '../../iBoneEditor';
import {IBone} from '../../../app-core/skeleton/ibone';
import {SkeletonEditorAnchorDirective} from '../skeleton-editor-anchor.directive';
import {BoneType} from 'src/contracts/bone-type';
import {TextBoneEditorComponent} from '../../text-bone-editor/text-bone-editor.component';
import {MockEditorComponent} from '../../mock-editor/mock-editor.component';
import {TextColumnsBoneEditorComponent} from '../../text-columns-bone-editor/text-columns-bone-editor.component';
import {OneMediaBoneEditorComponent} from '../../one-media-bone-editor/one-media-bone-editor.component';
import {DeviceType} from '../../DeviceType';
import {HeroMediaBoneEditorComponent} from '../../hero-media-bone-editor/hero-media-bone-editor.component';
import {VimeoEditorComponent} from '../../vimeo-editor/vimeo-editor.component';
import {ProjectCreditsEditorComponent} from '../../project-credits-editor/project-credits-editor.component';
import {SvgHeroEditorComponent} from '../../svg-hero-editor/svg-hero-editor.component';
import {PageHeaderEditorComponent} from '../../page-header-editor/page-header-editor.component';
import {ShowreelEditorComponent} from '../../showreel-editor/showreel-editor.component';
import {NewsEditorComponent} from '../../news-editor/news-editor.component';
import {LinksWithGraphicEditorComponent} from '../../links-with-graphic-editor/links-with-graphic-editor.component';
import {ButtonsAndTextEditorComponent} from '../../buttons-and-text-editor/buttons-and-text-editor.component';
import {MediaAndTextEditorComponent} from '../../media-and-text-editor/media-and-text-editor.component';
import {TileSliderEditorComponent} from '../../tile-slider-editor/tile-slider-editor.component';
import {Subscription} from 'rxjs';
import {DeviceVisibility} from "../../../app-core/skeleton/deviceVisibility";
import {hasFlag, setOrRemoveFlag} from "../../../../contracts/utils";

@Component({
  selector: 'bonc-bone-editor-container',
  templateUrl: './bone-editor-container.component.html',
  styleUrls: ['./bone-editor-container.component.scss']
})
export class BoneEditorContainerComponent implements OnChanges {
  @ViewChild(SkeletonEditorAnchorDirective, {static: true})
  public anchor: SkeletonEditorAnchorDirective;

  @Output()
  public removed: EventEmitter<void> = new EventEmitter<void>();

  @Output()
  public saved: EventEmitter<IBone> = new EventEmitter<IBone>();

  @Output()
  public editing: EventEmitter<boolean> = new EventEmitter<boolean>();

  public DeviceType = DeviceType;

  public editor?: IBoneEditor;

  public themePopupIsShown = false;

  private _bone: IBone;

  private removeSubscription?: Subscription;
  private saveSubscription?: Subscription;
  private changedSubscription?: Subscription;

  @Input()
  public locale: string;

  @Input()
  public device = DeviceType.NotSet;

  @HostBinding('style.--bg-color')
  public get bgColor(): string | undefined {
    return this.editor?.bone.colorTheme?.background;
  }

  @HostBinding('style.--text-color')
  public get textColor(): string | undefined {
    return this.editor?.bone.colorTheme?.text;
  }

  @HostBinding('style.--noticeable-color')
  public get noticeableTextColor(): string | undefined {
    return this.editor?.bone.colorTheme?.noticeable;
  }

  @HostBinding('style.--media-bg')
  public get mediaBgColor(): string | undefined {
    return this.editor?.bone.colorTheme?.mediaBg;
  }

  constructor(private readonly componentFactoryResolver: ComponentFactoryResolver) {
  }

  ngOnChanges(): void {
    if (this.editor === undefined || this.editor === null)
      return;

    this.editor.locale = this.locale;
    this.editor.device = this.device;
  }

  public get bone(): IBone {
    return this._bone;
  }

  @Input()
  public set bone(newBone: IBone) {
    this._bone = newBone;
    this.editor = undefined;

    if (this.removeSubscription) {
      this.removeSubscription.unsubscribe();
      this.removeSubscription = undefined;
    }

    if (this.saveSubscription) {
      this.saveSubscription.unsubscribe();
      this.saveSubscription = undefined;
    }

    if (this.changedSubscription) {
      this.changedSubscription.unsubscribe();
      this.changedSubscription = undefined;
    }

    const viewContainerRef = this.anchor.viewContainerRef;

    viewContainerRef.clear();

    if (newBone === undefined || newBone === null)
      return;

    const componentType = this.getComponent(newBone.type);

    const componentFactory = this.componentFactoryResolver.resolveComponentFactory(componentType);
    const boneEditorRef = viewContainerRef.createComponent(componentFactory);

    this.editor = boneEditorRef.instance;
    this.editor.bone = newBone;

    this.removeSubscription = this.editor.removed.subscribe(() => {
      this.removed.next();
    });

    this.changedSubscription = this.editor.editing.subscribe
    (
      (isEditing: boolean) => {
        this.editing.next(isEditing);
      }
    );

    this.saveSubscription = this.editor.saved.subscribe(
      (newBoneValue: IBone) => {
        this.saved.next(newBoneValue);
      }
    );

    this.ngOnChanges();
  }

  private getComponent(type: BoneType): Type<IBoneEditor> {
    switch (type) {

      case BoneType.Text:
        return TextBoneEditorComponent;

      case BoneType.TextColumns:
        return TextColumnsBoneEditorComponent;

      case BoneType.OneMedia:
        return OneMediaBoneEditorComponent;

      case BoneType.HeroMedia:
        return HeroMediaBoneEditorComponent;

      case BoneType.Vimeo:
        return VimeoEditorComponent;

      case BoneType.ProjectCredits:
        return ProjectCreditsEditorComponent;

      case BoneType.SvgHero:
        return SvgHeroEditorComponent;

      case BoneType.PageHeader:
        return PageHeaderEditorComponent;

      case BoneType.Showreel:
        return ShowreelEditorComponent;

      case BoneType.News:
        return NewsEditorComponent;

      case BoneType.LinksWithGraphic:
        return LinksWithGraphicEditorComponent;

      case BoneType.ButtonAndText:
        return ButtonsAndTextEditorComponent;

      case BoneType.MediaAndText:
        return MediaAndTextEditorComponent;

      case BoneType.TileSlider:
        return TileSliderEditorComponent;

      default:
        return MockEditorComponent;
    }
  }

  public nextPreset = (): void => {
    if (this.editor === undefined || this.editor === null)
      return;

    this.editor.nextPreset();
  }

  public setDisabled = (disabled: boolean): void => {
    if (this.editor === undefined || this.editor === null)
      return;

    this.editor.startEditing();

    if (this.device === DeviceType.Desktop)
      this.editor.bone.visibility = setOrRemoveFlag(this.editor.bone.visibility, DeviceVisibility.Desktop, !disabled);
    else if (this.device === DeviceType.Tablet)
      this.editor.bone.visibility = setOrRemoveFlag(this.editor.bone.visibility, DeviceVisibility.Tablet, !disabled);
    else if (this.device === DeviceType.Mobile)
      this.editor.bone.visibility = setOrRemoveFlag(this.editor.bone.visibility, DeviceVisibility.Mobile, !disabled);

    this.editor.updateDirty();
  }

  public get disabled(): boolean {
    if (this.editor === undefined || this.editor === null)
      throw new Error('нет эдитора в get disabled()');

    const visibility = this.editor.bone.visibility;

    if (this.device === DeviceType.Desktop && hasFlag(visibility, DeviceVisibility.Desktop))
      return false;

    if (this.device === DeviceType.Tablet && hasFlag(visibility, DeviceVisibility.Tablet))
      return false;

    if (this.device === DeviceType.Mobile && hasFlag(visibility, DeviceVisibility.Mobile))
      return false;

    return true;
  }

  public showThemePopup(): void {
    if (this.editor === undefined || this.editor === null)
      return;

    if (this.editor.bone.colorTheme === undefined || this.editor.bone.colorTheme === null)
      this.editor.bone.colorTheme = DEFAULT_COLOR_THEME();

    this.themePopupIsShown = true;
  }

  public hideThemePopup(): void {
    this.themePopupIsShown = false;
  }

  public resetTheme(editor: IBoneEditor): void {
    if (this.editor === undefined || this.editor === null)
      return;

    this.editor.bone.colorTheme = undefined;

    editor.startEditing();
    this.hideThemePopup();
  }
}
