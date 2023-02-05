using System.Collections.Generic;
using UnityEngine;

public class AncestorGenerator : MonoBehaviour {

    public static AncestorGenerator Instance;

    [SerializeField] private ItemDatabase itemDatabase;

    private void Awake() {
        Instance = this;
    }

#region Level Design

    public (AncestorData, AncestorData) GetInitialParents() {
        return (GenerateAncestor(1), GenerateAncestor(1));
    }

    public (AncestorData, AncestorData) GetParents(AncestorData node) {
        int parentsLevel = node.Level + 1;
        return (GenerateAncestor(parentsLevel), GenerateAncestor(parentsLevel));
    }

    public AncestorData GenerateAncestor(int level) {
        return new AncestorData(
            name: GenerateAncestorName(),
            level: level
        );
    }

#endregion

    public Dictionary<PlayerStats, int> GenerateBossStats(int bossLevel) {
        return new Dictionary<PlayerStats, int>() {
            {PlayerStats.Strength, 100 + bossLevel * 20},
            {PlayerStats.MagicPower, 50 + bossLevel * 10},
            {PlayerStats.AttackSpeed, 100 + bossLevel * 20},
            {PlayerStats.MovementSpeed, 100 + bossLevel * 20},
            {PlayerStats.Defense, 20 + bossLevel * 5},
            {PlayerStats.MaxHp, 1000 + bossLevel * 100}
        };
    }

    public Dictionary<ItemSlot, Item> GenerateBossEquipment(int bossLevel) {
        return new Dictionary<ItemSlot, Item>() {
            {ItemSlot.Head, itemDatabase.GetRandomItem(ItemKind.Helmet)},
            {ItemSlot.Torso, itemDatabase.GetRandomItem(ItemKind.Armor)},
            {ItemSlot.LeftArm, itemDatabase.GetRandomItem(ItemKind.Weapon)},
            {ItemSlot.RightArm, itemDatabase.GetRandomItem(ItemKind.Weapon)},
            {ItemSlot.Ring1, itemDatabase.GetRandomItem(ItemKind.Ring)}
        };
    }

    private string GenerateAncestorName() {
        int r = Random.Range(0, namePool.Length);
        return namePool[r];
    }

    private static readonly string[] namePool = new string[] {
        "SORRIBAS",
       "GASSO",
       "LEANDRO",
       "BOBADILLA",
       "INGELMO",
       "HILL",
       "LACAMBRA",
       "CASTANEDO",
       "ESCLAPEZ",
       "PARAMIO",
       "SOL",
       "SUBIAS",
       "ALOMAR",
       "EL HADDAD",
       "EGUIZABAL",
       "MOCHOLI",
       "VILLAPLANA",
       "MORE",
       "CASAUS",
       "LINO",
       "CIENFUEGOS",
       "BOTA",
       "DE LARA",
       "SOSPEDRA",
       "SAN ANDRES",
       "HORTA",
       "LECUMBERRI",
       "SOBRAL",
       "ALMARZA",
       "CHIA",
       "PECHARROMAN",
       "CALDENTEY",
       "COVELO",
       "TAVARES",
       "LAREDO",
       "HERRERAS",
       "ESTESO",
       "FLORENSA",
       "ETXABE",
       "FUNEZ",
       "AMIL",
       "CARRERES",
       "ENGUIX",
       "ROSENDE",
       "ILLANES",
       "CANTADOR",
       "ABADIAS",
       "ALBELDA",
       "NOGUER",
       "URSU",
       "ARAMENDI",
       "MOMPEAN",
       "ACEÑA",
       "SALAR",
       "ARAMENDIA",
       "RAMBLA",
       "CALDAS",
       "URIBARRI",
       "BUSTELO",
       "ALEN",
       "OLIVO",
       "ILIEV",
       "BUIZA",
       "GAVIRIA",
       "ARTIME",
       "PEDRERA",
       "CASARRUBIOS",
       "FORCADELL",
       "SEISDEDOS",
       "REYNES",
       "BEGUM",
       "EL ASRI",
       "SIRBU",
       "ELICES",
       "GRASA",
       "PIÑAS",
       "BENAGES",
       "CORRALIZA",
       "EVANGELISTA",
       "COSTACHE",
       "JARDON",
       "PALLEJA",
       "JUARISTI",
       "RUFINO",
       "PARRALES",
       "CALAFELL",
       "COSME",
       "BAGUENA",
       "REALES",
       "ENSEÑAT",
       "CEBRIA",
       "CAYUELAS",
       "SHAHZAD",
       "AGUINAGA",
       "BALMASEDA",
       "MUNNE",
       "MOLPECERES",
       "RUBIN",
       "YORDANOVA",
       "SOMOLINOS",
       "DORRONSORO",
       "MONTEALEGRE",
       "TABUENCA",
       "AZOFRA",
       "CARRASQUILLA",
       "HERGUEDAS",
       "OREJUELA",
       "PELLITERO",
       "CALAFAT",
       "MORCUENDE",
       "PORTUGUES",
       "PAYERAS",
       "ECHAVARRI",
       "GUILLOT",
       "CANEDA",
       "COTAN",
       "CARCAMO",
       "HORNOS",
       "RABAL",
       "DIAGNE",
       "GERPE",
       "ALMENA",
       "ARROJO",
       "DENGRA",
       "LINDO",
       "CARRIZOSA",
       "GUALLAR",
       "CAÑIBANO",
       "OLID",
       "ZUBIETA",
       "MONTELONGO",
       "CINTADO",
       "TORMOS",
       "CALLADO",
       "PAGOLA",
       "BERTOLIN",
       "COLETO",
       "AÑO",
       "GAVILANES",
       "RIAZA",
       "FERNANDEZ PACHECO",
       "MASFERRER",
       "AZPIROZ",
       "CLUA",
       "MAGRANER",
       "ARDURA",
       "CUÑADO",
       "BACIU",
       "PORTAL",
       "MORIANO",
       "TOBIAS",
       "MADRUGA",
       "LOZADA",
       "BORGE",
       "DUMITRESCU",
       "NEGREIRA",
       "TERRES",
       "LLOVERAS",
       "RENGIFO",
       "SUAU",
       "MARCELINO",
       "STOYANOV",
       "CASASNOVAS",
       "ARILLA",
       "ALMIRALL",
       "TOQUERO",
       "BRINGAS",
       "DRAME",
       "WRIGHT",
       "YEPEZ",
       "CALAHORRO",
       "CANDELARIO",
       "OTEO",
       "PARENTE",
       "SOLIÑO",
       "COTILLAS",
       "VILLAMIL",
       "ZAHARIA",
       "ECHAVE",
       "FRANCIA",
       "TORA",
       "MUNICIO",
       "JOAQUIN",
       "DACAL",
       "ALCORTA",
       "MARDONES",
       "AMARILLA",
       "SHI",
       "RAMO",
       "FALAGAN",
       "GAVIN",
       "LIBERAL",
       "TOURIÑO",
       "GUISASOLA",
       "COUÑAGO",
       "YUSTA",
       "SIMAL",
       "BRIZUELA",
       "CABELLOS",
       "ALAMILLO",
       "GALILEA",
       "MAICAS",
       "VILLACAÑAS",
       "CALLAU",
       "CHAPELA",
       "VILLAVICENCIO",
       "MASSANA",
       "VICIANA",
       "GARZA",
       "TRIANO",
       "CORROTO",
       "SERENO",
       "FORONDA",
       "DA CRUZ",
       "MEIJIDE",
       "NEDELCU",
       "GOROSTIZA",
       "NOGUE",
       "VILLUENDAS",
       "CUNILL",
       "ARAGONESES",
       "TAIBO",
       "ONRUBIA",
       "MIÑAMBRES",
       "ILIEVA",
       "AYESTARAN",
       "SEVA",
       "HIPOLITO",
       "ERASO",
       "GESTO",
       "DASILVA",
       "ARCHILLA",
       "VILLORA",
       "ALVITE",
       "PESO",
       "ALBAREDA",
       "BARRIL",
       "REMACHA",
       "ERRASTI",
       "MOVILLA",
       "MOREL",
       "MARINOV",
       "BAILO",
       "BURGUILLOS",
       "ZAS",
       "ARBOS",
       "UNZUETA",
       "BEIRO",
       "CAROL",
       "JACKSON",
       "LAMELAS"
    };

}