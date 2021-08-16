# High-Bay-Storage-Rack

The High-Bay-Storage-Rack (*Hochregallager*)
supports to services:
Taking pieces in and out of stock.
As an argument the ID of the stock place
(`1` - `6`) must be provided.

## Internal Architecture

The Python modul [`hbsr.py`](./hbsr.py) provides an
`HighRiseWarehouse` class that contains the internal
methods.

### `store_in(pos)`

Loads a piece from the loader into the stock position `pos`
(form `1` to `6`).

### `store_out(pos)`

Loads the piece form the stock position `pos` (from `1` to `6`)
into the loader.
