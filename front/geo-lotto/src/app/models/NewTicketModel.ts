export class NewTicketModel {
    constructor(lng: number, lat: number, rollId: number) {
        this.Longitude = lng;
        this.Latitude = lat;
        this.RollId = rollId;
    }

    RollId: number;
    Longitude: number;
    Latitude: number;
}