using System;
using UnityEngine;
[Serializable]
public class RegisterModeResponse
{
    public bool status;
    public Account data;

}
[Serializable]

public class Account
{
    public int Id;
    public string Email;
    public string Password;
    public string Name;
    public DateTime Created_At;
    public DateTime Updated_At;
}
