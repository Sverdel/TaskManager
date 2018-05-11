export class Task {
    public id: number;
    public name: string;
    public stateId: number;
    public priorityId: number;
    public planedTimeCost: number = 1;
    public actualTimeCost: number = 1;
    public remainingTimeCost: number = 1;
    public createDateTime: string;
    public changeDatetime: string;
    public description: string;
    public userId: string;

    constructor(userId?: string) {
        if (userId)
        {
            this.userId = userId;
        }

        this.stateId = 1;
        this.priorityId = 1;
    }
}