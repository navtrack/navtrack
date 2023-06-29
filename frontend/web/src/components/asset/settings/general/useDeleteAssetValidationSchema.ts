import { object, ObjectSchema, string } from "yup";
import { DeleteAssetFormValues } from "./types";
import { useCurrentAsset } from "@navtrack/shared/hooks/assets/useCurrentAsset";

export function useDeleteAssetValidationSchema() {
  const currentAsset = useCurrentAsset();

  const validationSchema: ObjectSchema<DeleteAssetFormValues> = object()
    .shape({
      name: string()
        .required("generic.name.required")
        .equals(
          [`${currentAsset.data?.name}`],
          "assets.settings.general.delete-asset.name-match"
        )
    })
    .defined();

  return validationSchema;
}
