export class PropertyValidationResult {
  errors: string[] = [];

  AddError(message: string): void {
    this.errors.push(message);
  }

  Clear(): void {
    this.errors = [];
  }

  HasErrors(): boolean {
    return this.errors.length > 0;
  }
}
