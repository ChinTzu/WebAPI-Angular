import { Entity } from "../../shared/models/entity";

export class Note extends Entity {
  title: string;
  body: string;
  username: string;
  updateTime: Date;
  remark: string;
}
