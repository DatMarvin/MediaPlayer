Public Class PlayerEnums

    Enum PlayMode
        STRAIGHT
        REPEAT
        RANDOM
    End Enum
    Enum LoopMode
        NO
        INTERMEDIATE
        YES
    End Enum
    Enum FirstStartState
        INIT
        STARTING
        STARTED
    End Enum

    Enum SearchState
        NONE
        INIT
        EMPTY
        SEARCHING
    End Enum

    Enum sortMode
        REVERSE = 1
        NAME = 0
        DATE_ADDED = 2
        TIME_LISTENED = 4
        COUNT = 6
        LENGTH = 8
        POPULARITY = 10
    End Enum

    Enum MusicSource
        LOCAL
        RADIO
    End Enum


End Class
