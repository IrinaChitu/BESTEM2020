                                                     ◄♠♣♦♥GAME RULES♥♦♣♠►

TBA

                                                    ◄♠♣♦♥CARD ANATOMY♥♦♣♠►

TBA - IMG

                                                      ◄♠♣♦♥GAMEBOARD♥♦♣♠►

TBA - IMG

                                                       ◄♠♣♦♥HEROES♥♦♣♠►

TBA

                                                      ◄♠♣♦♥CREATURES♥♦♣♠►

TBA

                                                       ◄♠♣♦♥SPELLS♥♦♣♠►
TBA

                                                       ◄♠♣♦♥ASSETS♥♦♣♠►
[Assets](https://damstorageapi.ubisoft.com/api/v1/proxy/c48e4b9f-613f-4adf-9614-2ce96806d27c/compression/6092802d-5c59-4653-884d-74c04d64dbcc?compressionLevel=Optimal&correlationId=e6d2b1a60aa446e0ae91a1d8556c5767&temp_url_sig=efb4a474c9e42fa114161662586766d07989115e&temp_url_expires=1605347272)


Layout

Zona 1: deck + cartile din mana (hand) + badge-ul eroului (display hero image, HP, Mana) + discard pile (on click arata cartile discarded ale jucatorului pana acum, i.e. spelluri consumate + unitati jucate).

Zona 2: Zona de spelluri. Cartile de spell puse in joc de catre jucator, pot fi active (tapped) sau inactive (untapped).

Zona 3: battlefield. 6 celule in care se afla unitati.

Gameplay
La inceputul unei runde incepe unul din playeri. Initial ales random, apoi alternativ incep cei doi rundele ulterioare.

La prima runda toti au P carti. La inceputul unei noi runde, trag o carte.

La prima runda jucatorii au la dispozitie un mana pool de valoare K (vedem cat). La fiecare runda mana pool-ul redevine full si ii creste capacitatea cu +1 (va creste pana la o limita superioare, apoi va ramane constant pana la finalul meciului).

Un erou consuma mana pentru a pune un spell. Spellurile au un numar de charges. Se poate consuma un charge al unui spell pentru a-l activa pentru fight-ul care va avea loc runda aceasta (tap). La runda urmatoare, daca mai exista chargeuri spellul devine untapped, altfel este discarded. Operatia de tap consuma un numar constant de mana (1?).

Jucatorul poate pune unitati in prima sa celula din battlefield (prima din stanga pentru verde) consumand mana. Unitatile sunt de tip melee sau ranged.  Ele au HP si Attack power. In plus, unele unitati pot avea Taunt (vezi mai jos).

Odata ce un jucator si-a jucat toate mutarile pe care le dorea poate da end turn si revine randul celuilalt. Dupa ce amandoi si-au facut mutarile din turn-ul curent, daca exista celule adiacente cu trupe diferite acestea incep un atac.

Un atac functioneaza astfel:
mai intai ataca unitatile melee, se calculeaza suma attack powerului pentru toate unitatile melee ale ambilor jucatori. Se sorteaza pentru amandoi unitatile melee descrescator dupa HP, prioritate avand cele cu Taunt. Folosind suma de attack power a unui jucator incep sa se elimine greedy unitatile melee ale adversarului in ordinea sortarii, cat timp exista AP la dispozitie. Daca o unitate ajunge la 0 HP dispare.
apoi ataca unitatile ranged, se calculeaza aceeasi suma de attack power, dar acum se pot ataca si unitati melee si ranged ale adversarului (acestea se sorteaza impreuna acum)

Exceptie: daca una din celulele implicate in battle se afla in spatele unui zid battle-ul se schimba astfel:
unitatile din castel ataca in mod normal unitatile inamice
unitatile din afara castelului dau tot damage-ul in erou (nu pot da damage unitatilor din spatele zidului)

Dupa o lupta, daca doar una din celule devine goala, jucatorul caruia i-au ramas unitati in cealalta celula, deplaseaza battle line-ul mai aproape de inamic cu o unitate (battle lineul nu poate trece mai departe de linia zidului). La finalul rundei, toate trupele avanseaza cu cate o casuta, cu conditia de a nu depasi battle line-ul. 


Spells

increase HP/AP range/melee, modifica statsurile unitatilor implicate in battle pentru o runda
scorched earth: distruge toate unitatile melee din battle (both players)
spears of heaven: distruge toate unitatile ranged din battle (both players)
suicide squad: dupa lupta, daca un jucator ramane fara unitati, iar celalalt cu, cel care maia re unitati le poate sacrifica pentru a da damage eroului inamic egal cu suma AP-urilor acestor unitati

Hero abilities (consuma mana)
+k HP pentru erou valabil doar o runda
+k ranged damage in plus dat de zid pentru o runda
convert all melee units to range for a fight
at the end of a round if mana is available draw one more card.

Carte:
Unitate
imagine
mana
nume
HP
AP
Melee/Range
Taunt (daca e)
Spell
imagine
mana
nume
descriere


