using System.Threading.Tasks;
using UnityEngine;

public abstract class Service : MonoBehaviour
{
    public abstract Task InitializeAsync();
}