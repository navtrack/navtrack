import { useCallback } from "react";
import { FormikHelpers } from "formik";
import { mapErrors } from "../../utils/formik";
import { useAddAssetMutation } from "../queries/assets/useAddAssetMutation";
import { Entity } from "../../api/model/generated";
import { useCurrentOrganization } from "../current/useCurrentOrganization";
import { ObjectSchema, object, string } from "yup";

export type CreateAssetFormValues = {
  name: string;
  deviceTypeId: string;
  serialNumber: string;
};

export const DefaultCreateAssetFormValues: CreateAssetFormValues = {
  name: "",
  deviceTypeId: "",
  serialNumber: ""
};

type UseCreateAssetProps = {
  onSuccess: (data: Entity) => void;
};

export function useCreateAsset(props: UseCreateAssetProps) {
  const addAssetMutation = useAddAssetMutation();
  const currentOrganization = useCurrentOrganization();

  const handleSubmit = useCallback(
    (
      values: CreateAssetFormValues,
      formikHelpers: FormikHelpers<CreateAssetFormValues>
    ) => {
      if (currentOrganization.data) {
        addAssetMutation.mutate(
          {
            organizationId: currentOrganization.data.id,
            data: {
              name: values.name,
              serialNumber: values.serialNumber,
              deviceTypeId: values.deviceTypeId
            }
          },
          {
            onSuccess: (asset) => props.onSuccess(asset),
            onError: (error) => mapErrors(error, formikHelpers)
          }
        );
      }
    },
    [addAssetMutation, currentOrganization.data, props]
  );

  const validationSchema: ObjectSchema<CreateAssetFormValues> = object({
    name: string().required("generic.name.required"),
    deviceTypeId: string().required("generic.device-type.required"),
    serialNumber: string().required("generic.serial-number.required")
  }).defined();

  return {
    handleSubmit,
    isLoading: addAssetMutation.isLoading,
    validationSchema
  };
}
