/**
 * Promomash
 * No description provided (generated by Openapi Generator https://github.com/openapitools/openapi-generator)
 *
 * The version of the OpenAPI document: 1.0.0
 * 
 *
 * NOTE: This class is auto generated by OpenAPI Generator (https://openapi-generator.tech).
 * https://openapi-generator.tech
 * Do not edit the class manually.
 */
import { HttpHeaders }                                       from '@angular/common/http';

import { Observable }                                        from 'rxjs';

import { ChangeNameCommand2 } from '../model/models';
import { CreateProvinceCommand } from '../model/models';
import { GetProvinceQueryResponse } from '../model/models';
import { GetProvincesQueryResponse } from '../model/models';
import { ProblemDetails } from '../model/models';


import { Configuration }                                     from '../configuration';



export interface ProvinceServiceInterface {
    defaultHeaders: HttpHeaders;
    configuration: Configuration;

    /**
     * 
     * 
     * @param changeNameCommand2 
     */
    provinceChangeName(changeNameCommand2: ChangeNameCommand2, extraHttpRequestParams?: any): Observable<{}>;

    /**
     * 
     * 
     * @param createProvinceCommand 
     */
    provinceCreate(createProvinceCommand: CreateProvinceCommand, extraHttpRequestParams?: any): Observable<{}>;

    /**
     * 
     * 
     * @param id 
     */
    provinceDelete(id: string, extraHttpRequestParams?: any): Observable<{}>;

    /**
     * 
     * 
     * @param id 
     */
    provinceGet(id: string, extraHttpRequestParams?: any): Observable<GetProvinceQueryResponse>;

    /**
     * 
     * 
     */
    provinceGetProvinces(extraHttpRequestParams?: any): Observable<Array<GetProvincesQueryResponse>>;

}
