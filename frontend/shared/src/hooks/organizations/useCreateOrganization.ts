import { useCallback } from "react";
import { FormikHelpers } from "formik";
import { mapErrors } from "../../utils/formik";
import { Entity } from "../../api/model";
import { ObjectSchema, object, string } from "yup";
import { useCreateOrganizationMutation } from "../queries/organizations/useCreateOrganizationMutation";

export type CreateOrganizationFormValues = {
  name: string;
};

export const DefaultCreateOrganizationFormValues: CreateOrganizationFormValues =
  {
    name: ""
  };

export function useCreateOrganization() {
  const createOrganizationMutation = useCreateOrganizationMutation();

  const handleSubmit = useCallback(
    (
      values: CreateOrganizationFormValues,
      formikHelpers: FormikHelpers<CreateOrganizationFormValues>,
      onSuccess: (data: Entity) => void
    ) => {
      createOrganizationMutation.mutate(
        {
          data: {
            name: values.name
          }
        },
        {
          onSuccess: (result) => onSuccess(result),
          onError: (error) => mapErrors(error, formikHelpers)
        }
      );
    },
    [createOrganizationMutation]
  );

  const validationSchema: ObjectSchema<CreateOrganizationFormValues> = object({
    name: string().required("generic.name.required")
  }).defined();

  return {
    handleSubmit,
    isLoading: createOrganizationMutation.isPending,
    validationSchema
  };
}
