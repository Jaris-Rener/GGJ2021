using UnityEngine;

public class MessageHandler : MonoBehaviour
{
    public TextAsset MessageTemplates;
    public TextAsset ResponseTemplates;
    public TextAsset GoodResponseTemplates;
    public TextAsset Names;
    public Sprite[] Portraits;

    private ItemTag[] _tags;
    private string[] _names;
    private string[] _templates;
    private string[] _goodResponseTemplates;
    private string[] _responseTemplates;

    private void Start()
    {
        _responseTemplates = ResponseTemplates.text.Split('\n');
        _goodResponseTemplates = GoodResponseTemplates.text.Split('\n');
        _names = Names.text.Split('\n');
        _templates = MessageTemplates.text.Split('\n');
        _tags = Resources.LoadAll<ItemTag>("Tags");
    }

    public void GenerateResponse(Message message, bool correctTag)
    {
        var template = correctTag
            ? _goodResponseTemplates.RandomElement().Trim()
            : _responseTemplates.RandomElement().Trim();

        var msg = template.Replace("<tag>", $"<b>{message.Tag.Name}</b>").Replace("<name>", message.Sender);

        var response = new Message
        {
            Text = msg,
            Sender = message.Sender,
            Tag = message.Tag,
            Portrait = message.Portrait
        };

        print($"Reply From: {response.Sender}\n" +
              $"> {response.Text}\n" +
              $"({response.Tag})");
    }

    [ContextMenu("Generate")]
    public Message GenerateRequest()
    {
        var template = _templates.RandomElement().Trim();
        var sender = _names.RandomElement();
        var itemTag = _tags.RandomElement();
        var portrait = Portraits.RandomElement();
        var msg = template.Replace("<tag>", $"<color=#0FC><b>{itemTag.Name}</b></color>").Replace("<name>", sender);

        var request = new Message
        {
            Text = msg,
            Sender = sender,
            Tag = itemTag,
            Portrait = portrait
        };

        print($"Message From: {request.Sender}\n" +
              $"> {request.Text}\n" +
              $"({request.Tag})");

        return request;
    }
}

public struct Message
{
    public string Text;
    public string Sender;
    public ItemTag Tag;
    public Sprite Portrait;
}
