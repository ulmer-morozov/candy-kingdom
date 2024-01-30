import { Component, Host, Input, OnChanges, OnInit, QueryList, SimpleChanges, ViewChildren } from '@angular/core';

import { BoneEditorContainerComponent } from './bone-editor-container/bone-editor-container.component';
import { Bone } from '@candy-kingdom/bonnie';
import { EditableDirective } from '../core-components';
import { DeviceType } from '../core';

interface IBoneTemplate {
  readonly title: string;
  readonly boneFactory: () => Bone;
}

function template<T>(title: string, type: string, dataEtalon: T): IBoneTemplate {
  return {
    title,
    boneFactory: () => ({
      type,
      data: JSON.parse(JSON.stringify(dataEtalon)),
      visibility: DeviceVisibility.All
    })
  };
}

@Component({
  selector: 'bonc-skeleton-editor',
  templateUrl: './skeleton-editor.component.html',
  styleUrls: ['./skeleton-editor.component.scss'],
  hostDirectives: [EditableDirective]
})
export class SkeletonEditorComponent implements OnInit, OnChanges {
  @ViewChildren('boneEditorContainer')
  public boneEditorContainerList!: QueryList<BoneEditorContainerComponent>;

  @Input({ required: true })
  public locale!: string;

  @Input()
  public device = DeviceType.NotSet;

  public readonly templatesAreShown: boolean[] = [];

  public readonly templates: ReadonlyArray<IBoneTemplate>;

  constructor(@Host() public editable: EditableDirective<IBone[]>) {
    this.templates = [
      template<MediaBoneData>('Big image or video', BoneType.OneMedia, {
        style: OneMediaStyle.ControlledBasic,
        src: emptyImagePack(),
        title: emptyLocalizedString(),
        text: emptyLocalizedString(),
        alt: emptyLocalizedString(),
        link: emptyLocalizedString()
      }),
      //
      template<IVimeoContentData>('Vimeo.com', BoneType.Vimeo, {
        vimeoId: 0,
        ratio: 16 / 9,
        loop: false,
        muted: false,
        autoplay: false
      }),
      //
      template<TextBoneData>('Text', BoneType.Text, {
        text: emptyLocalizedString(),
        title: emptyLocalizedString(),
        title2: emptyLocalizedString(),
        style: TextBoneStyle.LeftBigShorter,
        columnTwo: emptyLocalizedString(),
        columnThree: emptyLocalizedString()
      })
    ];
  }

  ngOnInit(): void {
    this.editable.externalSaveCall.subscribe(() => {
      this.boneEditorContainerList.forEach
        (
          editorContainer => editorContainer.editor?.save()
        );

      this.editable.save();
    });

    this.editable.canceled.subscribe(() => {
      this.boneEditorContainerList.forEach
        (
          editorContainer => editorContainer.editor?.resetData()
        );

      this.editable.save();
    });
  }

  ngOnChanges(changes: SimpleChanges): void {
    this.templatesAreShown.splice(0, this.templatesAreShown.length);
    if (this.bones === undefined)
      return;

    this.bones.forEach(x => this.templatesAreShown.push(false));
    // и  для последнего элемента
    this.templatesAreShown.push(false);
  }

  public get bones(): IBone[] {
    return this.editable?.value ?? [];
  }

  public boneEditHandler(isEditing: boolean): void {
    if (isEditing)
      this.editable.startEditing();
    else {
      const allClosed = this.boneEditorContainerList
        .filter(x => x.editor !== undefined && x.editor.isEditing)
        .length === 0;

      this.editable.updateDirty();

      if (!this.editable.isDirty && allClosed)
        this.editable.close();
    }
  }

  public boneChangeHandler(index: number, newBoneValue: Bone): void {
    this.editable.value[index] = newBoneValue;
    this.editable.save();
  }

  public removeBone(boneIndex: number): void {
    if (this.bones === undefined || this.bones === null)
      return;

    if (!confirm('Вы уверены, что хотите удалить компонент?'))
      return;

    this.bones.splice(boneIndex, 1);

    if (!this.editable.inEditMode)
      this.editable.startEditing();

    this.editable.save();
  }

  public moveDown(boneIndex: number): void {
    if (boneIndex >= this.bones.length - 1)
      return;

    this.swapBones(boneIndex, boneIndex + 1);
  }

  public moveUp(boneIndex: number): void {
    if (boneIndex <= 0)
      return;

    this.swapBones(boneIndex - 1, boneIndex);
  }

  public createBone(index: number, selectedTemplate: IBoneTemplate): void {
    const newBone = selectedTemplate.boneFactory();

    this.bones.splice(index, 0, newBone);
  }

  private swapBones(index1: number, index2: number): void {
    if (index1 < 0 || index2 < 0 || index1 >= this.bones.length || index2 >= this.bones.length)
      throw new Error(`ошибка swapBones. неправильные индексы ${index1} и ${index2}. bone count: ${this.bones.length}`);

    const tempBone = this.bones[index1];
    this.bones[index1] = this.bones[index2];
    this.bones[index2] = tempBone;
  }
}


