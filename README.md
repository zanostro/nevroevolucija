# nevroevolucija
 seminarska naloga za maturo

Zaradi visoke programske zahtevnosti, se naj bi simulacija vedno izvajala v notranjem Unity uredniku; in sicer v scene nacinu.
(pravilen izgled konfiguracije je na sliki "privailnaKonfiguracijaZaZagonSimulacije")
Ob zagonu projekta prek urejevalnika lahko Unity izpiše napako, v taki situaciji je potrebno samo pritisniti gumb quit in ponovno zagnati projekt

V projektu se tudi nahaja installer za Unity

Kako zagnati:
 1. zgoraj pritisniti gumb play
 2. pogled (zgoraj levod nad prikazom same igre) iz Game nastaviti na Scene (na tak način se lahko prosto opazujemo dogajanje in je veliko man stresno za računalnik)

 Kako zamenjati mapo:
 1. levo zgoraj izberemo modro pobarvani objekt "level Generator"
 2. na desni se nam odprejo njegove karakteristike
 3. pod "seed" vpisemo novo seme
 4. če želimo ob vsakem zagonu novo naključno generirano okolje, umaknemo knjukico pri "use seed"

Kako zamenjamo strukturo nevronske mreže:
1. levo spodaj imamo datoteko "scripts"
2. izberemo Global
3. pod segment //neuralNetwork imamo zapisano  public static int[] networkStructure = {rayCount, 10, 2};
4. prvi in zadnij element morata ostati fiksna, po želji pa lahko dodajamo poljubno število vmesnih slojev tako, da samo dodamo element (npr. {rayCount, 10, 10, 2})
5. številka 10 pomeni število nevronov v 1. nevidnem sloju

Kako nastavimo število vzorcev:
 1. levo zgoraj izberemo modro pobarvani objekt "level Generator"
 2. na desni se nam odprejo njegove karakteristike
 3. pod "Sample Size" vpišemo željeno velikost (minimalno je 20)


Kako ugasniti raycast žarke:
 1. levo spodaj izberemo datoteko prefabs
 2. spodaj izberemo model modrega avtomobila po imenom "player"
 3. levo zgoraj se nam odprejo podobjekti objekta avto
 4. izberemo "Lower Body"
 5. na desni označimo kvadratek "draw lines"
 6. levo je zraven modrega kvadra in napisa "player" puščica, da se vrnemo nazaj na prikaz polja
 7. žarki se bodo prižgali/ugasnili ob nastanku nove generacije


OPOMBE:
 1. širina in dolžina ograje je fiksna, tako da večanje in manjšanje mape lahko pokvari program
 2. če se nevronska mreža znajde v zelo globokem lokalnem minimumu lahko ponovno zaženemo simulacijo
 3. če simulacijo pavziramo moramo po nadaljevanju imulacije ponovno izbrati "scene" način
