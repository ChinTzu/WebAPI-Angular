import { QueryParameters } from "../../shared/query-parameters";

export class NoteParameters extends QueryParameters {
  title?: string;

  constructor(init?: Partial<NoteParameters>) {
    super(init);
    Object.assign(this, init);
  }
}
