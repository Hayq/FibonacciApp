# FibonacciApp

!Note this

The last thing I didn't do was caching the elements of the sequence, which will speeds up the generation of numbers. This means that for example I need to generate from 300 to 400, so in this case I could skip generating the first 300 numbers and start generating from 298 and 299 elements instead of the first index. I could store it in InMemoryCache, and when SkipCache is false, I could first check for needed items in InMemoryCache and, if found, start generating from that index.
The reason why I didn't, so I ran into some problem and could understand correctly. That's why I wrote this note.
