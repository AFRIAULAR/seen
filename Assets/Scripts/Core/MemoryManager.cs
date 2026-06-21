using UnityEngine;

public class MemoryManager : MonoBehaviour
{
    public static MemoryManager Instance;

    [Header("Hitos del juego")]
    public bool bancoVisitado;
    public bool codigoCasinoIngresado;
    public bool casinoJugado;
    public bool diarioUsado;
    public bool musicaEscuchada;

    [Header("Estado final")]
    public bool galeriaDesbloqueada;

    private const int RECUERDOS_NECESARIOS = 5;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void MarcarBancoVisitado()
    {
        if (bancoVisitado) return;

        bancoVisitado = true;
        RevisarGaleria();
    }

    public void MarcarCodigoCasinoIngresado()
    {
        if (codigoCasinoIngresado) return;

        codigoCasinoIngresado = true;
        RevisarGaleria();
    }

    public void MarcarCasinoJugado()
    {
        if (casinoJugado) return;

        casinoJugado = true;
        RevisarGaleria();
    }

    public void MarcarDiarioUsado()
    {
        if (diarioUsado) return;

        diarioUsado = true;
        RevisarGaleria();
    }

    public void MarcarMusicaEscuchada()
    {
        if (musicaEscuchada) return;

        musicaEscuchada = true;
        RevisarGaleria();
    }

    private void RevisarGaleria()
    {
        int recuerdos = ContarRecuerdos();

        Debug.Log("Recuerdos recuperados: " + recuerdos + "/" + RECUERDOS_NECESARIOS);

        if (recuerdos >= RECUERDOS_NECESARIOS && !galeriaDesbloqueada)
        {
            galeriaDesbloqueada = true;

            if (GalleryManager.Instance != null)
            {
                GalleryManager.Instance.DesbloquearGaleria();
            }

            Debug.Log("GALERÍA DESBLOQUEADA");
        }
    }

    public int ContarRecuerdos()
    {
        int total = 0;

        if (bancoVisitado) total++;
        if (codigoCasinoIngresado) total++;
        if (casinoJugado) total++;
        if (diarioUsado) total++;
        if (musicaEscuchada) total++;

        return total;
    }
}