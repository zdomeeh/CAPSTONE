EXTERNAL RevealKey()
-> start

=== start ===

TÒ: È il mio letto…
È un pò in disordine.

PINO: Non è che vuoi guardare sotto?

+ [Si]
    -> look

+ [No]
    TÒ: Non ho tempo da perdere.
    -> END


=== look ===

TÒ: Vediamo…

PINO: Io non mi abbasserei…

TÒ: Lo so.

TÒ: ...

PINO: Allora?

TÒ: C’è qualcosa.

PINO: Una chiave?!

TÒ: Già.

~ RevealKey()

-> END