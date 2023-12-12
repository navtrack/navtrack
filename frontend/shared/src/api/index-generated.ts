/**
 * Generated by orval v6.12.1 🍺
 * Do not edit manually.
 * Navtrack.Api
 * OpenAPI spec version: 1.0.0
 */
import {
  useQuery,
  useMutation
} from '@tanstack/react-query'
import type {
  UseQueryOptions,
  UseMutationOptions,
  QueryFunction,
  MutationFunction,
  UseQueryResult,
  QueryKey
} from '@tanstack/react-query'
import type {
  RegisterAccountModel,
  ForgotPasswordModel,
  ResetPasswordModel,
  ChangePasswordModel,
  ListModelOfAssetModel,
  AssetModel,
  CreateAssetModel,
  UpdateAssetModel,
  ListModelOfDeviceModel,
  UpdateAssetDeviceModel,
  LocationListModel,
  AssetsLocationsGetListParams,
  DistanceReportListModel,
  AssetsReportsGetTimeDistanceReportParams,
  TripListModel,
  AssetsTripsGetListParams,
  ListModelOfAssetUserModel,
  ProblemDetails,
  CreateAssetUserModel,
  ListModelOfDeviceTypeModel,
  ListModelOfProtocolModel,
  UserModel,
  UpdateUserModel
} from './model/generated'
import { authAxiosInstance } from './authAxiosInstance';


type AwaitedInput<T> = PromiseLike<T> | T;

      type Awaited<O> = O extends AwaitedInput<infer T> ? T : never;


export const accountRegister = (
    registerAccountModel: RegisterAccountModel,
 ) => {
      return authAxiosInstance<void>(
      {url: `/account`, method: 'post',
      headers: {'Content-Type': 'application/json', },
      data: registerAccountModel
    },
      );
    }
  


    export type AccountRegisterMutationResult = NonNullable<Awaited<ReturnType<typeof accountRegister>>>
    export type AccountRegisterMutationBody = RegisterAccountModel
    export type AccountRegisterMutationError = unknown

    export const useAccountRegister = <TError = unknown,
    
    TContext = unknown>(options?: { mutation?:UseMutationOptions<Awaited<ReturnType<typeof accountRegister>>, TError,{data: RegisterAccountModel}, TContext>, }
) => {
      const {mutation: mutationOptions} = options ?? {};

      


      const mutationFn: MutationFunction<Awaited<ReturnType<typeof accountRegister>>, {data: RegisterAccountModel}> = (props) => {
          const {data} = props ?? {};

          return  accountRegister(data,)
        }

        

      return useMutation<Awaited<ReturnType<typeof accountRegister>>, TError, {data: RegisterAccountModel}, TContext>(mutationFn, mutationOptions);
    }
    
export const accountForgotPassword = (
    forgotPasswordModel: ForgotPasswordModel,
 ) => {
      return authAxiosInstance<void>(
      {url: `/user/password/forgot`, method: 'post',
      headers: {'Content-Type': 'application/json', },
      data: forgotPasswordModel
    },
      );
    }
  


    export type AccountForgotPasswordMutationResult = NonNullable<Awaited<ReturnType<typeof accountForgotPassword>>>
    export type AccountForgotPasswordMutationBody = ForgotPasswordModel
    export type AccountForgotPasswordMutationError = unknown

    export const useAccountForgotPassword = <TError = unknown,
    
    TContext = unknown>(options?: { mutation?:UseMutationOptions<Awaited<ReturnType<typeof accountForgotPassword>>, TError,{data: ForgotPasswordModel}, TContext>, }
) => {
      const {mutation: mutationOptions} = options ?? {};

      


      const mutationFn: MutationFunction<Awaited<ReturnType<typeof accountForgotPassword>>, {data: ForgotPasswordModel}> = (props) => {
          const {data} = props ?? {};

          return  accountForgotPassword(data,)
        }

        

      return useMutation<Awaited<ReturnType<typeof accountForgotPassword>>, TError, {data: ForgotPasswordModel}, TContext>(mutationFn, mutationOptions);
    }
    
export const accountResetPassword = (
    resetPasswordModel: ResetPasswordModel,
 ) => {
      return authAxiosInstance<void>(
      {url: `/user/password/reset`, method: 'post',
      headers: {'Content-Type': 'application/json', },
      data: resetPasswordModel
    },
      );
    }
  


    export type AccountResetPasswordMutationResult = NonNullable<Awaited<ReturnType<typeof accountResetPassword>>>
    export type AccountResetPasswordMutationBody = ResetPasswordModel
    export type AccountResetPasswordMutationError = unknown

    export const useAccountResetPassword = <TError = unknown,
    
    TContext = unknown>(options?: { mutation?:UseMutationOptions<Awaited<ReturnType<typeof accountResetPassword>>, TError,{data: ResetPasswordModel}, TContext>, }
) => {
      const {mutation: mutationOptions} = options ?? {};

      


      const mutationFn: MutationFunction<Awaited<ReturnType<typeof accountResetPassword>>, {data: ResetPasswordModel}> = (props) => {
          const {data} = props ?? {};

          return  accountResetPassword(data,)
        }

        

      return useMutation<Awaited<ReturnType<typeof accountResetPassword>>, TError, {data: ResetPasswordModel}, TContext>(mutationFn, mutationOptions);
    }
    
export const accountChangePassword = (
    changePasswordModel: ChangePasswordModel,
 ) => {
      return authAxiosInstance<void>(
      {url: `/user/password/change`, method: 'post',
      headers: {'Content-Type': 'application/json', },
      data: changePasswordModel
    },
      );
    }
  


    export type AccountChangePasswordMutationResult = NonNullable<Awaited<ReturnType<typeof accountChangePassword>>>
    export type AccountChangePasswordMutationBody = ChangePasswordModel
    export type AccountChangePasswordMutationError = unknown

    export const useAccountChangePassword = <TError = unknown,
    
    TContext = unknown>(options?: { mutation?:UseMutationOptions<Awaited<ReturnType<typeof accountChangePassword>>, TError,{data: ChangePasswordModel}, TContext>, }
) => {
      const {mutation: mutationOptions} = options ?? {};

      


      const mutationFn: MutationFunction<Awaited<ReturnType<typeof accountChangePassword>>, {data: ChangePasswordModel}> = (props) => {
          const {data} = props ?? {};

          return  accountChangePassword(data,)
        }

        

      return useMutation<Awaited<ReturnType<typeof accountChangePassword>>, TError, {data: ChangePasswordModel}, TContext>(mutationFn, mutationOptions);
    }
    
export const assetsGetList = (
    
 signal?: AbortSignal
) => {
      return authAxiosInstance<ListModelOfAssetModel>(
      {url: `/assets`, method: 'get', signal
    },
      );
    }
  

export const getAssetsGetListQueryKey = () => [`/assets`];

    
export type AssetsGetListQueryResult = NonNullable<Awaited<ReturnType<typeof assetsGetList>>>
export type AssetsGetListQueryError = unknown

export const useAssetsGetList = <TData = Awaited<ReturnType<typeof assetsGetList>>, TError = unknown>(
  options?: { query?:UseQueryOptions<Awaited<ReturnType<typeof assetsGetList>>, TError, TData>, }

  ):  UseQueryResult<TData, TError> & { queryKey: QueryKey } => {

  const {query: queryOptions} = options ?? {};

  const queryKey =  queryOptions?.queryKey ?? getAssetsGetListQueryKey();

  


  const queryFn: QueryFunction<Awaited<ReturnType<typeof assetsGetList>>> = ({ signal }) => assetsGetList(signal);


  

  const query = useQuery<Awaited<ReturnType<typeof assetsGetList>>, TError, TData>({ queryKey, queryFn, ...queryOptions}) as  UseQueryResult<TData, TError> & { queryKey: QueryKey };

  query.queryKey = queryKey;

  return query;
}


export const assetsCreate = (
    createAssetModel: CreateAssetModel,
 ) => {
      return authAxiosInstance<AssetModel>(
      {url: `/assets`, method: 'post',
      headers: {'Content-Type': 'application/json', },
      data: createAssetModel
    },
      );
    }
  


    export type AssetsCreateMutationResult = NonNullable<Awaited<ReturnType<typeof assetsCreate>>>
    export type AssetsCreateMutationBody = CreateAssetModel
    export type AssetsCreateMutationError = unknown

    export const useAssetsCreate = <TError = unknown,
    
    TContext = unknown>(options?: { mutation?:UseMutationOptions<Awaited<ReturnType<typeof assetsCreate>>, TError,{data: CreateAssetModel}, TContext>, }
) => {
      const {mutation: mutationOptions} = options ?? {};

      


      const mutationFn: MutationFunction<Awaited<ReturnType<typeof assetsCreate>>, {data: CreateAssetModel}> = (props) => {
          const {data} = props ?? {};

          return  assetsCreate(data,)
        }

        

      return useMutation<Awaited<ReturnType<typeof assetsCreate>>, TError, {data: CreateAssetModel}, TContext>(mutationFn, mutationOptions);
    }
    
export const assetsGet = (
    assetId: string,
 signal?: AbortSignal
) => {
      return authAxiosInstance<AssetModel>(
      {url: `/assets/${assetId}`, method: 'get', signal
    },
      );
    }
  

export const getAssetsGetQueryKey = (assetId: string,) => [`/assets/${assetId}`];

    
export type AssetsGetQueryResult = NonNullable<Awaited<ReturnType<typeof assetsGet>>>
export type AssetsGetQueryError = unknown

export const useAssetsGet = <TData = Awaited<ReturnType<typeof assetsGet>>, TError = unknown>(
 assetId: string, options?: { query?:UseQueryOptions<Awaited<ReturnType<typeof assetsGet>>, TError, TData>, }

  ):  UseQueryResult<TData, TError> & { queryKey: QueryKey } => {

  const {query: queryOptions} = options ?? {};

  const queryKey =  queryOptions?.queryKey ?? getAssetsGetQueryKey(assetId);

  


  const queryFn: QueryFunction<Awaited<ReturnType<typeof assetsGet>>> = ({ signal }) => assetsGet(assetId, signal);


  

  const query = useQuery<Awaited<ReturnType<typeof assetsGet>>, TError, TData>({ queryKey, queryFn, enabled: !!(assetId), ...queryOptions}) as  UseQueryResult<TData, TError> & { queryKey: QueryKey };

  query.queryKey = queryKey;

  return query;
}


export const assetsUpdate = (
    assetId: string,
    updateAssetModel: UpdateAssetModel,
 ) => {
      return authAxiosInstance<void>(
      {url: `/assets/${assetId}`, method: 'post',
      headers: {'Content-Type': 'application/json', },
      data: updateAssetModel
    },
      );
    }
  


    export type AssetsUpdateMutationResult = NonNullable<Awaited<ReturnType<typeof assetsUpdate>>>
    export type AssetsUpdateMutationBody = UpdateAssetModel
    export type AssetsUpdateMutationError = unknown

    export const useAssetsUpdate = <TError = unknown,
    
    TContext = unknown>(options?: { mutation?:UseMutationOptions<Awaited<ReturnType<typeof assetsUpdate>>, TError,{assetId: string;data: UpdateAssetModel}, TContext>, }
) => {
      const {mutation: mutationOptions} = options ?? {};

      


      const mutationFn: MutationFunction<Awaited<ReturnType<typeof assetsUpdate>>, {assetId: string;data: UpdateAssetModel}> = (props) => {
          const {assetId,data} = props ?? {};

          return  assetsUpdate(assetId,data,)
        }

        

      return useMutation<Awaited<ReturnType<typeof assetsUpdate>>, TError, {assetId: string;data: UpdateAssetModel}, TContext>(mutationFn, mutationOptions);
    }
    
export const assetsDelete = (
    assetId: string,
 ) => {
      return authAxiosInstance<void>(
      {url: `/assets/${assetId}`, method: 'delete'
    },
      );
    }
  


    export type AssetsDeleteMutationResult = NonNullable<Awaited<ReturnType<typeof assetsDelete>>>
    
    export type AssetsDeleteMutationError = unknown

    export const useAssetsDelete = <TError = unknown,
    
    TContext = unknown>(options?: { mutation?:UseMutationOptions<Awaited<ReturnType<typeof assetsDelete>>, TError,{assetId: string}, TContext>, }
) => {
      const {mutation: mutationOptions} = options ?? {};

      


      const mutationFn: MutationFunction<Awaited<ReturnType<typeof assetsDelete>>, {assetId: string}> = (props) => {
          const {assetId} = props ?? {};

          return  assetsDelete(assetId,)
        }

        

      return useMutation<Awaited<ReturnType<typeof assetsDelete>>, TError, {assetId: string}, TContext>(mutationFn, mutationOptions);
    }
    
export const assetsDevicesGetList = (
    assetId: string,
 signal?: AbortSignal
) => {
      return authAxiosInstance<ListModelOfDeviceModel>(
      {url: `/assets/${assetId}/devices`, method: 'get', signal
    },
      );
    }
  

export const getAssetsDevicesGetListQueryKey = (assetId: string,) => [`/assets/${assetId}/devices`];

    
export type AssetsDevicesGetListQueryResult = NonNullable<Awaited<ReturnType<typeof assetsDevicesGetList>>>
export type AssetsDevicesGetListQueryError = unknown

export const useAssetsDevicesGetList = <TData = Awaited<ReturnType<typeof assetsDevicesGetList>>, TError = unknown>(
 assetId: string, options?: { query?:UseQueryOptions<Awaited<ReturnType<typeof assetsDevicesGetList>>, TError, TData>, }

  ):  UseQueryResult<TData, TError> & { queryKey: QueryKey } => {

  const {query: queryOptions} = options ?? {};

  const queryKey =  queryOptions?.queryKey ?? getAssetsDevicesGetListQueryKey(assetId);

  


  const queryFn: QueryFunction<Awaited<ReturnType<typeof assetsDevicesGetList>>> = ({ signal }) => assetsDevicesGetList(assetId, signal);


  

  const query = useQuery<Awaited<ReturnType<typeof assetsDevicesGetList>>, TError, TData>({ queryKey, queryFn, enabled: !!(assetId), ...queryOptions}) as  UseQueryResult<TData, TError> & { queryKey: QueryKey };

  query.queryKey = queryKey;

  return query;
}


export const assetsDevicesUpdate = (
    assetId: string,
    updateAssetDeviceModel: UpdateAssetDeviceModel,
 ) => {
      return authAxiosInstance<void>(
      {url: `/assets/${assetId}/devices`, method: 'post',
      headers: {'Content-Type': 'application/json', },
      data: updateAssetDeviceModel
    },
      );
    }
  


    export type AssetsDevicesUpdateMutationResult = NonNullable<Awaited<ReturnType<typeof assetsDevicesUpdate>>>
    export type AssetsDevicesUpdateMutationBody = UpdateAssetDeviceModel
    export type AssetsDevicesUpdateMutationError = unknown

    export const useAssetsDevicesUpdate = <TError = unknown,
    
    TContext = unknown>(options?: { mutation?:UseMutationOptions<Awaited<ReturnType<typeof assetsDevicesUpdate>>, TError,{assetId: string;data: UpdateAssetDeviceModel}, TContext>, }
) => {
      const {mutation: mutationOptions} = options ?? {};

      


      const mutationFn: MutationFunction<Awaited<ReturnType<typeof assetsDevicesUpdate>>, {assetId: string;data: UpdateAssetDeviceModel}> = (props) => {
          const {assetId,data} = props ?? {};

          return  assetsDevicesUpdate(assetId,data,)
        }

        

      return useMutation<Awaited<ReturnType<typeof assetsDevicesUpdate>>, TError, {assetId: string;data: UpdateAssetDeviceModel}, TContext>(mutationFn, mutationOptions);
    }
    
export const assetsDevicesDelete = (
    assetId: string,
    deviceId: string,
 ) => {
      return authAxiosInstance<void>(
      {url: `/assets/${assetId}/devices/${deviceId}`, method: 'delete'
    },
      );
    }
  


    export type AssetsDevicesDeleteMutationResult = NonNullable<Awaited<ReturnType<typeof assetsDevicesDelete>>>
    
    export type AssetsDevicesDeleteMutationError = unknown

    export const useAssetsDevicesDelete = <TError = unknown,
    
    TContext = unknown>(options?: { mutation?:UseMutationOptions<Awaited<ReturnType<typeof assetsDevicesDelete>>, TError,{assetId: string;deviceId: string}, TContext>, }
) => {
      const {mutation: mutationOptions} = options ?? {};

      


      const mutationFn: MutationFunction<Awaited<ReturnType<typeof assetsDevicesDelete>>, {assetId: string;deviceId: string}> = (props) => {
          const {assetId,deviceId} = props ?? {};

          return  assetsDevicesDelete(assetId,deviceId,)
        }

        

      return useMutation<Awaited<ReturnType<typeof assetsDevicesDelete>>, TError, {assetId: string;deviceId: string}, TContext>(mutationFn, mutationOptions);
    }
    
export const assetsLocationsGetList = (
    assetId: string,
    params?: AssetsLocationsGetListParams,
 signal?: AbortSignal
) => {
      return authAxiosInstance<LocationListModel>(
      {url: `/assets/${assetId}/locations`, method: 'get',
        params, signal
    },
      );
    }
  

export const getAssetsLocationsGetListQueryKey = (assetId: string,
    params?: AssetsLocationsGetListParams,) => [`/assets/${assetId}/locations`, ...(params ? [params]: [])];

    
export type AssetsLocationsGetListQueryResult = NonNullable<Awaited<ReturnType<typeof assetsLocationsGetList>>>
export type AssetsLocationsGetListQueryError = unknown

export const useAssetsLocationsGetList = <TData = Awaited<ReturnType<typeof assetsLocationsGetList>>, TError = unknown>(
 assetId: string,
    params?: AssetsLocationsGetListParams, options?: { query?:UseQueryOptions<Awaited<ReturnType<typeof assetsLocationsGetList>>, TError, TData>, }

  ):  UseQueryResult<TData, TError> & { queryKey: QueryKey } => {

  const {query: queryOptions} = options ?? {};

  const queryKey =  queryOptions?.queryKey ?? getAssetsLocationsGetListQueryKey(assetId,params);

  


  const queryFn: QueryFunction<Awaited<ReturnType<typeof assetsLocationsGetList>>> = ({ signal }) => assetsLocationsGetList(assetId,params, signal);


  

  const query = useQuery<Awaited<ReturnType<typeof assetsLocationsGetList>>, TError, TData>({ queryKey, queryFn, enabled: !!(assetId), ...queryOptions}) as  UseQueryResult<TData, TError> & { queryKey: QueryKey };

  query.queryKey = queryKey;

  return query;
}


export const assetsReportsGetTimeDistanceReport = (
    assetId: string,
    params?: AssetsReportsGetTimeDistanceReportParams,
 signal?: AbortSignal
) => {
      return authAxiosInstance<DistanceReportListModel>(
      {url: `/assets/${assetId}/reports/time-distance`, method: 'get',
        params, signal
    },
      );
    }
  

export const getAssetsReportsGetTimeDistanceReportQueryKey = (assetId: string,
    params?: AssetsReportsGetTimeDistanceReportParams,) => [`/assets/${assetId}/reports/time-distance`, ...(params ? [params]: [])];

    
export type AssetsReportsGetTimeDistanceReportQueryResult = NonNullable<Awaited<ReturnType<typeof assetsReportsGetTimeDistanceReport>>>
export type AssetsReportsGetTimeDistanceReportQueryError = unknown

export const useAssetsReportsGetTimeDistanceReport = <TData = Awaited<ReturnType<typeof assetsReportsGetTimeDistanceReport>>, TError = unknown>(
 assetId: string,
    params?: AssetsReportsGetTimeDistanceReportParams, options?: { query?:UseQueryOptions<Awaited<ReturnType<typeof assetsReportsGetTimeDistanceReport>>, TError, TData>, }

  ):  UseQueryResult<TData, TError> & { queryKey: QueryKey } => {

  const {query: queryOptions} = options ?? {};

  const queryKey =  queryOptions?.queryKey ?? getAssetsReportsGetTimeDistanceReportQueryKey(assetId,params);

  


  const queryFn: QueryFunction<Awaited<ReturnType<typeof assetsReportsGetTimeDistanceReport>>> = ({ signal }) => assetsReportsGetTimeDistanceReport(assetId,params, signal);


  

  const query = useQuery<Awaited<ReturnType<typeof assetsReportsGetTimeDistanceReport>>, TError, TData>({ queryKey, queryFn, enabled: !!(assetId), ...queryOptions}) as  UseQueryResult<TData, TError> & { queryKey: QueryKey };

  query.queryKey = queryKey;

  return query;
}


export const assetsTripsGetList = (
    assetId: string,
    params?: AssetsTripsGetListParams,
 signal?: AbortSignal
) => {
      return authAxiosInstance<TripListModel>(
      {url: `/assets/${assetId}/trips`, method: 'get',
        params, signal
    },
      );
    }
  

export const getAssetsTripsGetListQueryKey = (assetId: string,
    params?: AssetsTripsGetListParams,) => [`/assets/${assetId}/trips`, ...(params ? [params]: [])];

    
export type AssetsTripsGetListQueryResult = NonNullable<Awaited<ReturnType<typeof assetsTripsGetList>>>
export type AssetsTripsGetListQueryError = unknown

export const useAssetsTripsGetList = <TData = Awaited<ReturnType<typeof assetsTripsGetList>>, TError = unknown>(
 assetId: string,
    params?: AssetsTripsGetListParams, options?: { query?:UseQueryOptions<Awaited<ReturnType<typeof assetsTripsGetList>>, TError, TData>, }

  ):  UseQueryResult<TData, TError> & { queryKey: QueryKey } => {

  const {query: queryOptions} = options ?? {};

  const queryKey =  queryOptions?.queryKey ?? getAssetsTripsGetListQueryKey(assetId,params);

  


  const queryFn: QueryFunction<Awaited<ReturnType<typeof assetsTripsGetList>>> = ({ signal }) => assetsTripsGetList(assetId,params, signal);


  

  const query = useQuery<Awaited<ReturnType<typeof assetsTripsGetList>>, TError, TData>({ queryKey, queryFn, enabled: !!(assetId), ...queryOptions}) as  UseQueryResult<TData, TError> & { queryKey: QueryKey };

  query.queryKey = queryKey;

  return query;
}


export const assetsUsersGetList = (
    assetId: string,
 signal?: AbortSignal
) => {
      return authAxiosInstance<ListModelOfAssetUserModel>(
      {url: `/assets/${assetId}/users`, method: 'get', signal
    },
      );
    }
  

export const getAssetsUsersGetListQueryKey = (assetId: string,) => [`/assets/${assetId}/users`];

    
export type AssetsUsersGetListQueryResult = NonNullable<Awaited<ReturnType<typeof assetsUsersGetList>>>
export type AssetsUsersGetListQueryError = ProblemDetails

export const useAssetsUsersGetList = <TData = Awaited<ReturnType<typeof assetsUsersGetList>>, TError = ProblemDetails>(
 assetId: string, options?: { query?:UseQueryOptions<Awaited<ReturnType<typeof assetsUsersGetList>>, TError, TData>, }

  ):  UseQueryResult<TData, TError> & { queryKey: QueryKey } => {

  const {query: queryOptions} = options ?? {};

  const queryKey =  queryOptions?.queryKey ?? getAssetsUsersGetListQueryKey(assetId);

  


  const queryFn: QueryFunction<Awaited<ReturnType<typeof assetsUsersGetList>>> = ({ signal }) => assetsUsersGetList(assetId, signal);


  

  const query = useQuery<Awaited<ReturnType<typeof assetsUsersGetList>>, TError, TData>({ queryKey, queryFn, enabled: !!(assetId), ...queryOptions}) as  UseQueryResult<TData, TError> & { queryKey: QueryKey };

  query.queryKey = queryKey;

  return query;
}


export const assetsUsersCreate = (
    assetId: string,
    createAssetUserModel: CreateAssetUserModel,
 ) => {
      return authAxiosInstance<void>(
      {url: `/assets/${assetId}/users`, method: 'post',
      headers: {'Content-Type': 'application/json', },
      data: createAssetUserModel
    },
      );
    }
  


    export type AssetsUsersCreateMutationResult = NonNullable<Awaited<ReturnType<typeof assetsUsersCreate>>>
    export type AssetsUsersCreateMutationBody = CreateAssetUserModel
    export type AssetsUsersCreateMutationError = unknown

    export const useAssetsUsersCreate = <TError = unknown,
    
    TContext = unknown>(options?: { mutation?:UseMutationOptions<Awaited<ReturnType<typeof assetsUsersCreate>>, TError,{assetId: string;data: CreateAssetUserModel}, TContext>, }
) => {
      const {mutation: mutationOptions} = options ?? {};

      


      const mutationFn: MutationFunction<Awaited<ReturnType<typeof assetsUsersCreate>>, {assetId: string;data: CreateAssetUserModel}> = (props) => {
          const {assetId,data} = props ?? {};

          return  assetsUsersCreate(assetId,data,)
        }

        

      return useMutation<Awaited<ReturnType<typeof assetsUsersCreate>>, TError, {assetId: string;data: CreateAssetUserModel}, TContext>(mutationFn, mutationOptions);
    }
    
export const assetsUsersDelete = (
    assetId: string,
    userId: string,
 ) => {
      return authAxiosInstance<void>(
      {url: `/assets/${assetId}/users/${userId}`, method: 'delete'
    },
      );
    }
  


    export type AssetsUsersDeleteMutationResult = NonNullable<Awaited<ReturnType<typeof assetsUsersDelete>>>
    
    export type AssetsUsersDeleteMutationError = unknown

    export const useAssetsUsersDelete = <TError = unknown,
    
    TContext = unknown>(options?: { mutation?:UseMutationOptions<Awaited<ReturnType<typeof assetsUsersDelete>>, TError,{assetId: string;userId: string}, TContext>, }
) => {
      const {mutation: mutationOptions} = options ?? {};

      


      const mutationFn: MutationFunction<Awaited<ReturnType<typeof assetsUsersDelete>>, {assetId: string;userId: string}> = (props) => {
          const {assetId,userId} = props ?? {};

          return  assetsUsersDelete(assetId,userId,)
        }

        

      return useMutation<Awaited<ReturnType<typeof assetsUsersDelete>>, TError, {assetId: string;userId: string}, TContext>(mutationFn, mutationOptions);
    }
    
export const devicesGetList = (
    
 signal?: AbortSignal
) => {
      return authAxiosInstance<ListModelOfDeviceTypeModel>(
      {url: `/devices/types`, method: 'get', signal
    },
      );
    }
  

export const getDevicesGetListQueryKey = () => [`/devices/types`];

    
export type DevicesGetListQueryResult = NonNullable<Awaited<ReturnType<typeof devicesGetList>>>
export type DevicesGetListQueryError = unknown

export const useDevicesGetList = <TData = Awaited<ReturnType<typeof devicesGetList>>, TError = unknown>(
  options?: { query?:UseQueryOptions<Awaited<ReturnType<typeof devicesGetList>>, TError, TData>, }

  ):  UseQueryResult<TData, TError> & { queryKey: QueryKey } => {

  const {query: queryOptions} = options ?? {};

  const queryKey =  queryOptions?.queryKey ?? getDevicesGetListQueryKey();

  


  const queryFn: QueryFunction<Awaited<ReturnType<typeof devicesGetList>>> = ({ signal }) => devicesGetList(signal);


  

  const query = useQuery<Awaited<ReturnType<typeof devicesGetList>>, TError, TData>({ queryKey, queryFn, ...queryOptions}) as  UseQueryResult<TData, TError> & { queryKey: QueryKey };

  query.queryKey = queryKey;

  return query;
}


export const protocolsGetList = (
    
 signal?: AbortSignal
) => {
      return authAxiosInstance<ListModelOfProtocolModel>(
      {url: `/protocols`, method: 'get', signal
    },
      );
    }
  

export const getProtocolsGetListQueryKey = () => [`/protocols`];

    
export type ProtocolsGetListQueryResult = NonNullable<Awaited<ReturnType<typeof protocolsGetList>>>
export type ProtocolsGetListQueryError = unknown

export const useProtocolsGetList = <TData = Awaited<ReturnType<typeof protocolsGetList>>, TError = unknown>(
  options?: { query?:UseQueryOptions<Awaited<ReturnType<typeof protocolsGetList>>, TError, TData>, }

  ):  UseQueryResult<TData, TError> & { queryKey: QueryKey } => {

  const {query: queryOptions} = options ?? {};

  const queryKey =  queryOptions?.queryKey ?? getProtocolsGetListQueryKey();

  


  const queryFn: QueryFunction<Awaited<ReturnType<typeof protocolsGetList>>> = ({ signal }) => protocolsGetList(signal);


  

  const query = useQuery<Awaited<ReturnType<typeof protocolsGetList>>, TError, TData>({ queryKey, queryFn, ...queryOptions}) as  UseQueryResult<TData, TError> & { queryKey: QueryKey };

  query.queryKey = queryKey;

  return query;
}


export const userGet = (
    
 signal?: AbortSignal
) => {
      return authAxiosInstance<UserModel>(
      {url: `/user`, method: 'get', signal
    },
      );
    }
  

export const getUserGetQueryKey = () => [`/user`];

    
export type UserGetQueryResult = NonNullable<Awaited<ReturnType<typeof userGet>>>
export type UserGetQueryError = unknown

export const useUserGet = <TData = Awaited<ReturnType<typeof userGet>>, TError = unknown>(
  options?: { query?:UseQueryOptions<Awaited<ReturnType<typeof userGet>>, TError, TData>, }

  ):  UseQueryResult<TData, TError> & { queryKey: QueryKey } => {

  const {query: queryOptions} = options ?? {};

  const queryKey =  queryOptions?.queryKey ?? getUserGetQueryKey();

  


  const queryFn: QueryFunction<Awaited<ReturnType<typeof userGet>>> = ({ signal }) => userGet(signal);


  

  const query = useQuery<Awaited<ReturnType<typeof userGet>>, TError, TData>({ queryKey, queryFn, ...queryOptions}) as  UseQueryResult<TData, TError> & { queryKey: QueryKey };

  query.queryKey = queryKey;

  return query;
}


export const userUpdate = (
    updateUserModel: UpdateUserModel,
 ) => {
      return authAxiosInstance<void>(
      {url: `/user`, method: 'patch',
      headers: {'Content-Type': 'application/json', },
      data: updateUserModel
    },
      );
    }
  


    export type UserUpdateMutationResult = NonNullable<Awaited<ReturnType<typeof userUpdate>>>
    export type UserUpdateMutationBody = UpdateUserModel
    export type UserUpdateMutationError = unknown

    export const useUserUpdate = <TError = unknown,
    
    TContext = unknown>(options?: { mutation?:UseMutationOptions<Awaited<ReturnType<typeof userUpdate>>, TError,{data: UpdateUserModel}, TContext>, }
) => {
      const {mutation: mutationOptions} = options ?? {};

      


      const mutationFn: MutationFunction<Awaited<ReturnType<typeof userUpdate>>, {data: UpdateUserModel}> = (props) => {
          const {data} = props ?? {};

          return  userUpdate(data,)
        }

        

      return useMutation<Awaited<ReturnType<typeof userUpdate>>, TError, {data: UpdateUserModel}, TContext>(mutationFn, mutationOptions);
    }
    
export const healthGet = (
    
 signal?: AbortSignal
) => {
      return authAxiosInstance<void>(
      {url: `/health`, method: 'get', signal
    },
      );
    }
  

export const getHealthGetQueryKey = () => [`/health`];

    
export type HealthGetQueryResult = NonNullable<Awaited<ReturnType<typeof healthGet>>>
export type HealthGetQueryError = unknown

export const useHealthGet = <TData = Awaited<ReturnType<typeof healthGet>>, TError = unknown>(
  options?: { query?:UseQueryOptions<Awaited<ReturnType<typeof healthGet>>, TError, TData>, }

  ):  UseQueryResult<TData, TError> & { queryKey: QueryKey } => {

  const {query: queryOptions} = options ?? {};

  const queryKey =  queryOptions?.queryKey ?? getHealthGetQueryKey();

  


  const queryFn: QueryFunction<Awaited<ReturnType<typeof healthGet>>> = ({ signal }) => healthGet(signal);


  

  const query = useQuery<Awaited<ReturnType<typeof healthGet>>, TError, TData>({ queryKey, queryFn, ...queryOptions}) as  UseQueryResult<TData, TError> & { queryKey: QueryKey };

  query.queryKey = queryKey;

  return query;
}


