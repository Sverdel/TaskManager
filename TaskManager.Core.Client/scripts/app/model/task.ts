export class Task {
    public id: number;
    public name: string;
    public stateId: number;
    public priorityId: number;
    public planedTimeCost: number;
    public actualTimeCost: number;
    public remainingTimeCost: number;
    public createDateTime: string;
    public changeDatetime: string;
    public description: string;

    constructor(id?: number, name?: string) {
        this.id = id;
        this.name = name;
        this.stateId = 1;
        this.priorityId = 1;
    }
}