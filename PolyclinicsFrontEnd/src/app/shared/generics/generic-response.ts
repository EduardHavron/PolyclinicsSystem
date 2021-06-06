export class GenericResponse<T, TE> {
  public isSuccess: boolean = false;
  public errors: Array<T> = new Array<T>()
  public result: TE;

  public constructor(isSuccess: boolean,
                     errors: Array<T>,
                     result: TE) {
    this.isSuccess = isSuccess;
    this.errors = errors;
    this.result = result;
  }
}
