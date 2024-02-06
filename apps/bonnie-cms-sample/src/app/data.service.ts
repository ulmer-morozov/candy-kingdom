import { HttpClient } from "@angular/common/http";
import { Inject, Injectable } from "@angular/core";
import { Observable, combineLatest, map, merge, mergeMap, of } from 'rxjs';

import { APP_BASE_HREF } from '@angular/common';
import { PageBase, View } from '@candy-kingdom/bonnie';

type DataDictionary = { [n: string]: Observable<unknown> };

@Injectable()
export class DataService {
  constructor(private readonly http: HttpClient, @Inject(APP_BASE_HREF) private baseHref: string) {
  }

  public getView(viewCode: string): Observable<Readonly<PageBase>> {
    const pageOb = this.getSkeleton<PageBase>(`${this.baseHref}api/views/${viewCode}`);
    return pageOb;
  }

  public getPage(pageRoute: string): Observable<Readonly<PageBase>> {
    const pageUrl = `/${pageRoute}`

    const pageOb = this.getSkeleton<PageBase>(`${this.baseHref}api/pages/?url=${encodeURIComponent(pageUrl)}`);
    return pageOb;
  }

  private getSkeleton<T extends (PageBase | View)>(url: string): Observable<Readonly<T>> {
    const skeletonOb = this.http.get<T>(url);

    const routeDataObs = skeletonOb
      .pipe
      (
        map(
          x => {
            const notEmptyDataRoutes = x.bones
              .map(b => ('dataRoute' in b) && typeof (b.dataRoute) === 'string' ? b.dataRoute : '')
              .filter(x => x.length > 0);

            if (notEmptyDataRoutes.length === 0) {
              const emptyData: DataDictionary = {};
              return { page: of(x), data: of(emptyData) };
            }

            return {
              page: of(x),
              data: combineLatest
                (
                  notEmptyDataRoutes
                    .map(dataRoute => {
                      const url = `${this.baseHref}api/Pages/Children/?url=${dataRoute}`;

                      return {
                        route: dataRoute,
                        json: this.http.get(url)
                      }
                    })
                    .reduce(
                      (prev, curr) => {
                        prev[curr.route] = curr.json;
                        return prev;
                      }, {} as DataDictionary
                    )
                )
            }
          }

        ),
        map(x => combineLatest(x)),
        mergeMap(res => merge(res)),
        map(x => {

          for (const bone of x.page.bones) {
            if (!('dataRoute' in bone) || typeof bone.dataRoute !== 'string' || bone.dataRoute.length === 0)
              continue;

            const data = x.data[bone.dataRoute];

            if (data === undefined || data === null)
              throw new Error(`Data ${bone.dataRoute} have not been preloaded`);

            (bone as any).data = data; // todo: fix
          }

          return x.page;
        })
      );

    return routeDataObs;
  }
}

