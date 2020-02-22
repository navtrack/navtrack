export type LocationHistoryRequestModel = {
    assetId: number;
    startDate: string;
    endDate: string;
    latitude?: number;
    longitude?: number;
    radius?: number;
    startSpeed?: number;
    endSpeed?: number;
    startAltitude?: number;
    endAltitude?: number;
};
