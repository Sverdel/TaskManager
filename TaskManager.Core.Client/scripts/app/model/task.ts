export class Task {
    public Id: number;
    public Name: string;
    public StateId: number;
    public PriorityId: number;
    public PlanedTimeCost: number;
    public ActualTimeCost: number;
    public RemainingTimeCost: number;
    public CreateDateTime: string;
    public ChangeDatetime: string;
    public Description: string;

    constructor(id: number, name: string) {
        this.Id = id;
        this.Name = name;
    }
}