namespace DsPair.src.Models;
public class ErrorMessage {
  public string type { get; }
  public string message { get; }
  public ErrorMessage(string message, string type) {
    this.message = message;
    this.type = type;
  }

}
