export class Task {
    public id: number | undefined;
    public name: string | undefined;
    public stateId: number;
    public priorityId: number;
    public planedTimeCost: number = 1;
    public actualTimeCost: number = 1;
    public remainingTimeCost: number = 1;
    public createDateTime: string | undefined;
    public changeDatetime: string | undefined;
    public description: string | undefined;
    public userId: string | undefined;

    constructor(userId?: string) {
        if (userId)
        {
            this.userId = userId;
        }

        this.stateId = 1;
        this.priorityId = 1;
    }
}