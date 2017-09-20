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